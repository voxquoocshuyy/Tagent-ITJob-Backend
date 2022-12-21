using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.TransactionRepositories;
using ITJob.Entity.Repositories.WalletRepositories;
using ITJob.Services.Services.VNPay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/VNPay")]
public class VNPayController : ControllerBase
{
  private readonly double _exchangeRate;
  private readonly IConfiguration _configuration;
  private readonly IWalletRepository _walletRepository;
  private readonly ITransactionRepository _transactionRepository;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="configuration"></param>
  /// <param name="walletRepository"></param>
  /// <param name="transactionRepository"></param>
  public VNPayController(IConfiguration configuration, IWalletRepository walletRepository, ITransactionRepository transactionRepository)
  {
    _configuration = configuration;
    _walletRepository = walletRepository;
    _transactionRepository = transactionRepository;
    _exchangeRate = double.Parse(configuration["SystemConfiguration:ExchangeRate"]);
  }

  /// <summary>
  /// [Guest] Endpoint for company create url payment with condition
  /// </summary>
  /// <param name="businessPayment">An object payment</param>
  /// <returns>List of user</returns>
  /// <response code="200">Returns the list of user</response>
  /// <response code="204">Returns if list of user is empty</response>
  /// <response code="403">Return if token is access denied</response>
  [HttpGet]
  [AllowAnonymous]
  public async Task<IActionResult> Get([FromQuery] BusinessPayment businessPayment)
  {
    string url = _configuration["VnPay:Url"];
    string returnUrl = _configuration["VnPay:ReturnAdminPath"];
    string tmnCode = _configuration["VnPay:TmnCode"];
    string hashSecret = _configuration["VnPay:HashSecret"];
    VnPayLibrary pay = new VnPayLibrary();

    pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.0.0
    pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
    pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
    pay.AddRequestData("vnp_Amount", businessPayment.Amount + "00"); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
    pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
    pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
    pay.AddRequestData("vnp_IpAddr", businessPayment.Ip); //Địa chỉ IP của khách hàng thực hiện giao dịch
    pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
    pay.AddRequestData("vnp_OrderInfo", businessPayment.CompanyId.ToString()); //Thông tin mô tả nội dung thanh toán
    pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
    pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
    pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn
    pay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddHours(12).ToString("yyyyMMddHHmmss")); //Thời gian kết thúc thanh toán
    string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

    return Ok(paymentUrl);
  }

  /// <summary>
  /// [Guest] Endpoint for company confirm payment with condition
  /// </summary>
  /// <returns>List of user</returns>
  /// <response code="200">Returns the list of user</response>
  /// <response code="204">Returns if list of user is empty</response>
  /// <response code="403">Return if token is access denied</response>
  [HttpGet("PaymentConfirm")]
  public async Task<IActionResult> Confirm()
  {
    string returnUrl = _configuration["VnPay:ReturnPath"];
    float amount = 0;
    string status = "failed";
    if (Request.Query.Count > 0)
    {
      string vnp_HashSecret = _configuration["VnPay:HashSecret"]; //Secret key
      var vnpayData = Request.Query;
      VnPayLibrary vnpay = new VnPayLibrary();
      foreach (string s in vnpayData.Keys)
      {
        //get all querystring data
        if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
        {
          vnpay.AddResponseData(s, vnpayData[s]);
        }
      }
      //Lay danh sach tham so tra ve tu VNPAY
      //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
      //vnp_TransactionNo: Ma GD tai he thong VNPAY
      //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
      //vnp_SecureHash: HmacSHA512 cua du lieu tra ve

      long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
      float vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
      amount = vnp_Amount;
      long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
      string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
      string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
      String vnp_SecureHash = Request.Query["vnp_SecureHash"];
      bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
      var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
      Guid companyId = Guid.Parse(vnp_OrderInfo);
      //Cap nhat ket qua GD
      //Yeu cau: Truy van vao CSDL cua  system => lay ra duoc Wallet
      //get from DB
      var wallet =
          await _walletRepository.GetFirstOrDefaultAsync(w => w.CompanyId == companyId);

      if (wallet != null)
      {
        if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
        {
          //Thanh toán thành công
          wallet.Balance += vnp_Amount / _exchangeRate;
          _walletRepository.Update(wallet);
          await _walletRepository.SaveChangesAsync();
          // returnContent = returnSuccessUrl;
          status = "success";
          var transaction = new Transaction
          {
            Total = vnp_Amount / _exchangeRate,
            TypeOfTransaction = "Money recharge",
            CreateBy = companyId,
            WalletId = wallet.Id
          };
          await _transactionRepository.InsertAsync(transaction);
          await _transactionRepository.SaveChangesAsync();
        }
      }
    }

    return Redirect(returnUrl + "?amount=" + amount + "&status=" + status);
  }
}