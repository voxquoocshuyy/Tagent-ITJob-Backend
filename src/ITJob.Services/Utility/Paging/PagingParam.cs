using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.Services.Utility.Paging;

public class PagingParam<TKey> where TKey : System.Enum
{
    private int _page = PagingConstant.FixedPagingConstant.DefaultPage;

    /// <summary>
    /// Gets or sets current page number.
    /// </summary>

    public int Page
    {
        get => _page;
        set => _page = (value);
    }

    /// <summary>
    /// Gets or sets size of current page.
    /// </summary>
    [FromQuery(Name = "page-size")]
    [DefaultValue(PagingConstant.FixedPagingConstant.DefaultPageSize)]
    public int PageSize { get; set; } = PagingConstant.FixedPagingConstant.DefaultPageSize;


    [Description("Parameter use for sorting result. Value: {propertyName}")]
    [FromQuery(Name = "sort-key")]
    public TKey? SortKey { get; set; } = default(TKey?);

    /// <summary>
    /// Gets or sets ordering criteria.
    /// </summary>
    [EnumDataType(typeof(PagingConstant.OrderCriteria))]
    [JsonConverter(typeof(PagingConstant.OrderCriteria))]
    [FromQuery(Name = "sort-order")]
    public PagingConstant.OrderCriteria SortOrder { get; set; }
}