namespace ITJob.Services.Enum;

public class UserEnum
{
    public enum UserStatus
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
        /// Status for verify
        /// </summary>
        Verifying = 2,
        
        /// <summary>
        /// Status for verified
        /// </summary>
        Verified = 3,
        
        /// <summary>
        /// Status for pending
        /// </summary>
        Pending = 4,
    }
    public enum UserSort
    {
        Phone,
        Email
    }
}