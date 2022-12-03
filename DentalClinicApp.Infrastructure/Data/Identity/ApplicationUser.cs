using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static DentalClinicApp.Infrastructure.Data.Constants.DataConstant.ApplicationUser;

namespace DentalClinicApp.Infrastructure.Data.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [MaxLength(MaxUserFirstName)]

        public string? FirstName { get; set; }

        [MaxLength(MaxUserLastName)]

        public string? LastName { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
