namespace DentalClinicApp.Core.Models.Users
{
	public class UserQueryServiceModel
	{
		public int TotalUsersCount { get; set; }

		public IList<UserListViewModel> Users { get; set; } = new List<UserListViewModel>();
	}
}
