namespace ITJob.Services.Enum;

public static class ApplicantEnum
{
    public enum ApplicantStatus
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
        /// Status for verifying
        /// </summary>
        Verifying = 2,
    }
    public enum ApplicantEarnMoney
    {
        /// <summary>
        /// Status for not earn money
        /// </summary>
        NotEarn = 0,
            
        /// <summary>
        /// Status for earn money
        /// </summary>
        Earn = 1,
        
        /// <summary>
        /// Status for pending earn money
        /// </summary>
        Pending = 2,
    }
    public enum ApplicantSort
    {
        Phone,
        Email,
        Status
    }
}