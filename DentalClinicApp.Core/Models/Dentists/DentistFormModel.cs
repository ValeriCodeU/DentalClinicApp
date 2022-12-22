using System.ComponentModel.DataAnnotations;
using static DentalClinicApp.Core.Constants.ModelConstant.User;

namespace DentalClinicApp.Core.Models.Dentists
{
    public class DentistFormModel
    {
        [Required]
        [StringLength(MaxUsernameLength, MinimumLength = MinUsernameLength)]

        public string UserName { get; set; } = null!;

        [Required]
        [StringLength(MaxFirstNameLength, MinimumLength = MinFirstNameLength)]

        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(MaxLastNameLength, MinimumLength = MinLastNameLength)]

        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(MaxEmaildLength, MinimumLength = MinEmailLength)]
        [EmailAddress]

        public string Email { get; set; } = null!;

        [Required]
        [Phone]

        public string PhoneNumber { get; set; } = null!;

        [Required]
        [StringLength(MaxPasswordLength, MinimumLength = MinPasswordLength)]
        [DataType(DataType.Password)]

        public string Password { get; set; } = null!;

        [Compare(nameof(Password))]
        [DataType(DataType.Password)]

        public string ConfirmPassword { get; set; } = null!;
    }
}
