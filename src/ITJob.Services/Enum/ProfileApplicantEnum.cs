namespace ITJob.Services.Enum;

public class ProfileApplicantEnum
{
    public enum ProfileApplicantStatus
    {
        /// <summary>
        /// Status for default
        /// </summary>
        Default = 0,
            
        /// <summary>
        /// Status for hidden
        /// </summary>
        Hidden = 1,
        
        /// <summary>
        /// Status for inactive
        /// </summary>
        Inactive = 2,
    }
    public enum ProfileApplicantSort
    {
        CreateDate,
        Education,
        JobPositionId,
        WorkingStyleId,
        ApplicantId,
        CertificateId
    }
}