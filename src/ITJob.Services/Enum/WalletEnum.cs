namespace ITJob.Services.Enum;

public class WalletEnum
{
    public enum WalletStatus
    {
        /// <summary>
        /// Status for deleted
        /// </summary>
        Inactive = 0,
            
        /// <summary>
        /// Status for active
        /// </summary>
        Active = 1
    }
    public enum WalletSort
    {
        CreateDate,
        Status
    }
}