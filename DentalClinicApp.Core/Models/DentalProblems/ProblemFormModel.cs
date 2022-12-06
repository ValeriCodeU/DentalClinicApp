using System.ComponentModel.DataAnnotations;
using static DentalClinicApp.Core.Constants.ModelConstant.DentalProblem;

namespace DentalClinicApp.Core.Models.DentalProblems
{
    public class ProblemFormModel
    {
        [Required]
        [StringLength(MaxDiseaseName, MinimumLength = MinDiseaseName)]

        public string DiseaseName { get; set; } = null!;

        [Required]
        [StringLength(MaxDiseaseDescription, MinimumLength = MinDiseaseDescription)]

        public string DiseaseDescription { get; set; } = null!;

        [Required]
        [StringLength(MaxDentalStatus, MinimumLength = MinDentalStatus)]

        public string DentalStatus { get; set; } = null!;

        [StringLength(MaxAlergyDescription, MinimumLength = MinAlergyDescription)]

        public string? AlergyDescription { get; set; }
        
    }
}
