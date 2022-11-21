using databatdongsan.helper;

namespace WebApplication.Models
{
    public class BaseViewModel
    {
        private string pageTitle;

        public string PageTitle { get => !string.IsNullOrWhiteSpace(pageTitle) ? pageTitle : ConstantHelper.PageTitle; set => pageTitle = value; }
        public string PreviousPage { get; set; }
        public string Keyword { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int OrderBy { get; set; }
        public int SortedBy { get; set; }
        public PaginationVM Pagination { get; set; }
    }
}