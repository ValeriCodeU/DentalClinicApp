namespace DentalClinicApp.Core.Models.Dentists
{
    public class DentistServiceModel
    {
        public IEnumerable<DentistModel> Dentists { get; set; } = new List<DentistModel>();
    }
}
