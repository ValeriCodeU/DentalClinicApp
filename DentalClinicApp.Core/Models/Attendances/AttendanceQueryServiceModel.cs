namespace DentalClinicApp.Core.Models.Attendances
{
    public class AttendanceQueryServiceModel
	{
        public int TotalAttendancesCount { get; set; }

        public IEnumerable<AttedanceServiceModel> Attendances { get; set; } = new List<AttedanceServiceModel>();
    }
}
