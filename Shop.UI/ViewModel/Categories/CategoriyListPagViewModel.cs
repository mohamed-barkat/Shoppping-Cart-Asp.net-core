using Shop.UI.Paginated;

namespace Shop.UI.ViewModel.Categories
{
    public class CategoriyListPagViewModel
    {

        public PaginatedList<CategoryListViewModel> categories { set; get; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { set; get; }
        public int PageIndex { get;  set; }
    }
}
