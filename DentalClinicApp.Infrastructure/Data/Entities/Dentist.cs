using DentalClinicApp.Infrastructure.Data.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalClinicApp.Infrastructure.Data.Entities
{
    public class Dentist
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]

        public ApplicationUser User { get; set; } = null!;

        public int ManagerId { get; set; }

        [ForeignKey(nameof(ManagerId))]

        public Manager Manager { get; set; } = null!;

        public ICollection<Patient> Patients { get; set; } = new List<Patient>();

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

        public ICollection<DentalProcedure> DentalProcedures { get; set; } = new List<DentalProcedure>();
    }
}
