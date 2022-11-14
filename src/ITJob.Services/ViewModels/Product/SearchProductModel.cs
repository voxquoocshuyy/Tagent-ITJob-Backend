using System.ComponentModel;

namespace ITJob.Services.ViewModels.Product;

public class SearchProductModel
{
    [DefaultValue("")]
    public string? Name { get; set; } = "";
    public double? Price { get; set; } = null;
    public int? Status { get; set; } = null;
}