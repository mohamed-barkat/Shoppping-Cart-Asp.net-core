using Shop.UI.Paginated;

namespace Shop.UI.ViewModel.Categories
{
    public class MyCategoriyListPagViewModel
    {

     //   public MyPaginatedList<CategoryListViewModel> categories { set; get; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { set; get; }
        public int PageIndex { get; set; }
    }
}
