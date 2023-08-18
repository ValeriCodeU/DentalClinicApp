namespace DentalClinicApp.Core.Models.Attendances
{
    public class AttendanceDetailsViewModel 
    {
        public int Id { get; set; }

        public string ClinicRemarks { get; set; } = null!;

        public string Diagnosis { get; set; } = null!;

        public string Date { get; set; } = null!;
    }
}
