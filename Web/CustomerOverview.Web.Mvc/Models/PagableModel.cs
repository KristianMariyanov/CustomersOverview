namespace CustomerOverview.Web.Mvc.Models
{
    public class PagableModel
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage => this.CurrentPage != 1;
    }
}
