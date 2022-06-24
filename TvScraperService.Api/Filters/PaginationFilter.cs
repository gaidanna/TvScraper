namespace TvScraperService.Api.Filters
{
    public class PaginationFilter
    {
        public const int MinPageNumber = 1;
        public const int MinPageSize = 1;
        public const int MaxPageSize = 10;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public void ValidatePaginationParams()
        {
            this.PageNumber = PageNumber < MinPageNumber ? MinPageNumber : PageNumber;
            this.PageSize = PageSize > MaxPageSize || PageSize < MinPageSize 
                ? MaxPageSize : PageSize;
        }
    }
}
