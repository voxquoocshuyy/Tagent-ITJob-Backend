namespace ITJob.Services.Enum;

public static class JobPostEnum
{
    public enum JobPostStatus
    {
        /// <summary>
        /// Status for deleted
        /// </summary>
        Default = 0,
            
        /// <summary>
        /// Status for active
        /// </summary>
        Hidden = 1,
        
        /// <summary>
        /// Status for pending
        /// </summary>
        Pending = 2,
        
        /// <summary>
        /// Status for cancel
        /// </summary>
        Cancel = 3,
        
        /// <summary>
        /// Status for posting
        /// </summary>
        Posting = 4,
    }
    public enum JobPostSort
    {
        Title,
        CompanyId,
        JobPositionId,
        WorkingStyleId,
        WorkingPlace,
        CreateDate,
        EndTime
    }
}