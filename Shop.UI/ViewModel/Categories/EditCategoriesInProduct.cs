using Shop.UI.Paginated;

namespace Shop.UI.ViewModel.Categories
{
    public class EditCategoriesInProduct
    {

        public PaginatedList<CategoryListViewModel> categories { set; get; }

        public int ProductId { set; get; }
        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { set; get; }
        public int PageIndex { get; set; }
    }
}
