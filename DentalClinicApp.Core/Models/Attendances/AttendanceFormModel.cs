using DentalClinicApp.Core.Models.Patients;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static DentalClinicApp.Core.Constants.ModelConstant.Attendance;

namespace DentalClinicApp.Core.Models.Attendances
{
    public class AttendanceFormModel
    {
        [Required]
        [StringLength(MaxClinicRemarksLength, MinimumLength = MinClinicRemarksLength)]
        [Display(Name = "Clinic Remarks")]

        public string ClinicRemarks { get; set; } = null!;

        [Required]
        [StringLength(MaxDiagnosislength, MinimumLength = MinDiagnosislength)]
        [Display(Name = "Diagnosis")]

        public string Diagnosis { get; set; } = null!;

        [Display(Name = "Select your patient")]

        public int PatientId { get; set; }

        public IEnumerable<PatientModel> Patients { get; set; } = new List<PatientModel>();
    }
}
