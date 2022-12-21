namespace ITJob.Services.Enum;

public class ProductEnum
{
    public enum ProductStatus
    {
        /// <summary>
        /// Status for active
        /// </summary>
        Active = 1,
        /// <summary>
        /// Status for inactive
        /// </summary>
        Inactive = 0
    }
    public enum ProductSort
    {
        Name,
        Price
    }
}