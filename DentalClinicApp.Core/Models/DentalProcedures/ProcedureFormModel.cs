using DentalClinicApp.Core.Models.Patients;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static DentalClinicApp.Core.Constants.ModelConstant.DentalProcedure;

namespace DentalClinicApp.Core.Models.DentalProcedures
{
    public class ProcedureFormModel
	{
        [Required]
        [StringLength(MaxProcedureName, MinimumLength = MinProcedureName)]

        public string Name { get; set; } = null!;

        [Required]
        [StringLength(MaxProcedureDescription, MinimumLength = MinProcedureDescription)]

        public string Description { get; set; } = null!;

        public string? Note { get; set; }
        
        public decimal Cost { get; set; }

        [Display(Name = "Select start date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]

        [Required]

        public string StartDate { get; set; } = null!;

        [Display(Name = "Select end date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]

        [Required]

        public string EndDate { get; set; } = null!;

        [Display(Name = "Select your patient")]

        public int PatientId { get; set; }

        public IEnumerable<PatientModel> Patients { get; set; } = new List<PatientModel>();
    }
}
