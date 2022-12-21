namespace ITJob.Services.Enum;

public class EmployeeEnum
{
    public enum EmployeeStatus
    {
        /// <summary>
        /// Status for inactive
        /// </summary>
        Inactive = 0,
            
        /// <summary>
        /// Status for active
        /// </summary>
        Active = 1
    }
    public enum EmployeeSort
    {
        Email,
        Phone,
        Status,
        Name
    }
}