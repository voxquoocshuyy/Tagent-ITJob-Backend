namespace ITJob.Services.ViewModels.Product;

public class GetProductDetail
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public double? Price { get; set; }
    public int? Quantity { get; set; }
    public int? Status { get; set; }
    public string? Image { get; set; }
}