using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Models.Users
{
    public class UserListViewModel
    {

        public Guid Id { get; set; } 

        [Required]

        public string Name { get; set; } = null!;

        [Required]

        public string Email { get; set; } = null!;

        public IEnumerable<string> RoleNames { get; set; } = new List<string>();
    }
}
