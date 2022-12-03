using DentalClinicApp.Infrastructure.Data.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalClinicApp.Infrastructure.Data.Entities
{
    public class Patient
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]

        public ApplicationUser User { get; set; } = null!;

        public int DentistId { get; set; }

        [ForeignKey(nameof(DentistId))]

        public Dentist Dentist { get; set; } = null!;

        public ICollection<DentalProblem> DentalProblems { get; set; } = new List<DentalProblem>();

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

        public ICollection<DentalProcedure> DentalProcedures { get; set; } = new List<DentalProcedure>();

    }
}
