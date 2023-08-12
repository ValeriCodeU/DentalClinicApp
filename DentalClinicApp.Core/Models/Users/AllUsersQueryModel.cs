using DentalClinicApp.Core.Models.Attendances;
using DentalClinicApp.Core.Models.Users.Enums;
using ShoppingListApp.Core.Models.Products.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DentalClinicApp.Core.Models.Users
{
	public class AllUsersQueryModel
	{
		public const int UsersPerPage = 10;

        public string? RoleName { get; set; }

        [Display(Name = "Search by text")]

        public string? SearchTerm { get; set; }

        public UserSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalUsersCount { get; set; }

        public IEnumerable<string> RoleNames = new List<string>();

        public IEnumerable<UserListViewModel> Users { get; set; } = new List<UserListViewModel>();
    }
}
