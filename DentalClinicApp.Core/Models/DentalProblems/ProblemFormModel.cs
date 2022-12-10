using System.ComponentModel.DataAnnotations;
using static DentalClinicApp.Core.Constants.ModelConstant.DentalProblem;

namespace DentalClinicApp.Core.Models.DentalProblems
{
    public class ProblemFormModel
    {
        [Required]
        [StringLength(MaxDiseaseName, MinimumLength = MinDiseaseName)]
        [Display(Name = "Disease Name")]

        public string DiseaseName { get; set; } = null!;

        [Required]
        [StringLength(MaxDiseaseDescription, MinimumLength = MinDiseaseDescription)]
        [Display(Name = "Description")]

        public string DiseaseDescription { get; set; } = null!;

        [Required]
        [StringLength(MaxDentalStatus, MinimumLength = MinDentalStatus)]
        [Display(Name = "Dental Status")]

        public string DentalStatus { get; set; } = null!;

        [StringLength(MaxAlergyDescription, MinimumLength = MinAlergyDescription)]
        [Display(Name = "Describe any allergies")]

        public string? AlergyDescription { get; set; }
        
    }
}
