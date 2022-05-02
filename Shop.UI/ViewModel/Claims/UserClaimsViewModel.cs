namespace Shop.UI.ViewModel.Claims
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            Claims = new List<UserClaim>();
        }
        public string userId { get; set; }
        public List<UserClaim> Claims { get; set; }
    }
}
