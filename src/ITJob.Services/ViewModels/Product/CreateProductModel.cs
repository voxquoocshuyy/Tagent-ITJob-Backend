using ITJob.Services.ViewModels.File;

namespace ITJob.Services.ViewModels.Product;

public class CreateProductModel : FileViewModel
{
    public string? Name { get; set; }
    public double? Price { get; set; }
    public int? Quantity { get; set; }
    public int? Status { get; set; }
}