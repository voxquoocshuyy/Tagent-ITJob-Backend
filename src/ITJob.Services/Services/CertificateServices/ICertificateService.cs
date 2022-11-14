using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Certificate;

namespace ITJob.Services.Services.CertificateServices;

public interface ICertificateService
{
    IList<GetCertificateDetail> GetCertificatePage(PagingParam<CertificateEnum.CertificateSort> paginationModel, SearchCertificateModel searchCertificateModel);

    public Task<GetCertificateDetail> GetCertificateById(Guid id);

    public Task<GetCertificateDetail> CreateCertificateAsync(CreateCertificateModel requestBody);

    public Task<GetCertificateDetail> UpdateCertificateAsync(Guid id, UpdateCertificateModel requestBody);

    public Task DeleteCertificateAsync(Guid id);

    public Task<int> GetTotal();   
}