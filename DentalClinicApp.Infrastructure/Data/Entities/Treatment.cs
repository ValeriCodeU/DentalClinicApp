using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DentalClinicApp.Infrastructure.Data.Constants.DataConstant.Treatment;

namespace DentalClinicApp.Infrastructure.Data.Entities
{
    public class Treatment
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxTreatmentName)]

        public string Name { get; set; } = null!;

        [MaxLength(MaxTreatmentDescription)]

        public string Description { get; set; } = null!;

        [Column(TypeName = "Money")]
        [Precision(TreatmentPrecisionDecimal, TreatmentScaleDecimal)]

        public decimal Cost { get; set; }

        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();        

        public int AttendanceId { get; set; }

        [ForeignKey(nameof(AttendanceId))]

        public Attendance Attendance { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
