using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DentalClinicApp.Infrastructure.Data.Constants.DataConstant.DentalProblem;

namespace DentalClinicApp.Infrastructure.Data.Entities
{
    public class DentalProblem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxDiseaseName)]

        public string DiseaseName { get; set; } = null!;

        [Required]
        [MaxLength(MaxDiseaseDescription)]

        public string DiseaseDescription { get; set; } = null!;

        [Required]
        [MaxLength(MaxDentalStatus)]

        public string DentalStatus { get; set; } = null!;

        [MaxLength(MaxAlergyDescription)]

        public string? AlergyDescription { get; set; }

        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]

        public Patient Patient { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
