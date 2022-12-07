namespace DentalClinicApp.Core.Models.DentalProblems
{
    public class ProblemDetailsViewModel
	{
		public int Id { get; set; }

        public string DiseaseName { get; set; } = null!;

        public string DiseaseDescription { get; set; } = null!;

        public string DentalStatus { get; set; } = null!;

        public string? AlergyDescription { get; set; }
    }
}
