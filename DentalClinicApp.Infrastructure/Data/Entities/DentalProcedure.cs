using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DentalClinicApp.Infrastructure.Data.Constants.DataConstant.DentalProcedure;

namespace DentalClinicApp.Infrastructure.Data.Entities
{
    public class DentalProcedure
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxProcedureName)]

        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(MaxProcedureDescription)]

        public string Description { get; set; } = null!;

        [MaxLength(MaxProcedureNote)]

        public string? Note { get; set; } 

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]

        public Patient Patient { get; set; } = null!;

        public int DentistId { get; set; }

        [ForeignKey(nameof(DentistId))]

        public Dentist Dentist { get; set; } = null!;

        [Column(TypeName = "Money")]
        [Precision(ProcedurePrecisionDecimal, ProcedureScaleDecimal)]

        public decimal Cost { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
