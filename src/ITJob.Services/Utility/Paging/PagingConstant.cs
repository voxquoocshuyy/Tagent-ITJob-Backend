namespace ITJob.Services.Utility.Paging;

/// <summary>
/// Contains constant classes related to Pagination.
/// </summary>
public static class PagingConstant
{
    
    /// <summary>
    /// Contains property length related to Paging
    /// </summary>
    public static class FixedPagingConstant
    {
        /// <summary>
        /// LimitationPageSize
        /// </summary>
        public const int MaxPageSize = 500;
        public const int DefaultPage = 1;
        public const int DefaultPageSize = 50;
        public const string DefaultSort = "id_asc";
        public const int MinPage = 0;
        public const int MinPageSize = 3;
    }
        
    public enum OrderCriteria
    {
        /// <summary>
        /// descendant
        /// </summary>
        DESC,

        /// <summary>
        /// ascendant
        /// </summary>
        ASC,
    }
}
