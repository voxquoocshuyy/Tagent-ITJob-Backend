namespace ITJob.Services.Enum;

public static class CompanyEnum
{
    public enum CompanyStatus
    {
        /// <summary>
        /// Status for deleted
        /// </summary>
        Inactive = 0,
            
        /// <summary>
        /// Status for active
        /// </summary>
        Active = 1,
        
        /// <summary>
        /// Status for pending
        /// </summary>
        Pending = 2,
    }
    public enum CompanyPremium
    {
        /// <summary>
        /// Status for not premium
        /// </summary>
        NotPremium = 0,
            
        /// <summary>
        /// Status for premium
        /// </summary>
        Premium = 1,
    }
    public enum CompanySort
    {
        Email,
        Phone,
        Website,
        Status,
        Name,
        Premium
    }
}