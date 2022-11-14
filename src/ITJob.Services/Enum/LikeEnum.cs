namespace ITJob.Services.Enum;

public class LikeEnum
{
    public enum Match
    {
        /// <summary>
        /// Status for match
        /// </summary>
        Match = 1,
            
        /// <summary>
        /// Status for not match
        /// </summary>
        NotMatch = 1,
    }
    public enum LikeSort
    {
        JobPostId,
        ProfileApplicantId,
        CreateDate,
        Match
    }
}