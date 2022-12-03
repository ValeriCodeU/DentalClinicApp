using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DentalClinicApp.Infrastructure.Data.Constants.DataConstant.Attendance;

namespace DentalClinicApp.Infrastructure.Data.Entities
{
    public class Attendance
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxClinicRemarksLength)]

        public string ClinicRemarks { get; set; } = null!;

        [Required]
        [MaxLength(MaxDiagnosislength)]

        public string Diagnosis { get; set; } = null!;

        public DateTime Date { get; set; }

        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]

        public Patient Patient { get; set; } = null!;

        public int DentistId { get; set; }

        [ForeignKey(nameof(DentistId))]

        public Dentist Dentist { get; set; } = null!;

        public ICollection<Treatment> Treatments { get; set; } = new List<Treatment>();

        public bool IsActive { get; set; } = true;

    }
}
