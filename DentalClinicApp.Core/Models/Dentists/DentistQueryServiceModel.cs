namespace DentalClinicApp.Core.Models.Dentists
{
    public class DentistQueryServiceModel
	{
        public int TotalDentistsCount { get; set; }

        public IList<DentistServiceModel> Dentists { get; set; } = new List<DentistServiceModel>();
    }
}
