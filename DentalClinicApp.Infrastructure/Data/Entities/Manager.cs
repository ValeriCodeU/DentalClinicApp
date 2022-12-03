using DentalClinicApp.Infrastructure.Data.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalClinicApp.Infrastructure.Data.Entities
{
    public class Manager
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]

        public ApplicationUser User { get; set; } = null!;

        public ICollection<Dentist> AcceptedDentists { get; set; } = new List<Dentist>();
    }
}
