namespace ITJob.Services.ViewModels.Company;

public class GetCompanyDetail
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Logo { get; set; } = null!;
    public string Website { get; set; } = null!;
    public int? Status { get; set; } = null!;
    public bool? IsPremium { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? CompanyType { get; set; }
    public string? TaxCode { get; set; }
    public string? Reason { get; set; }
}