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

        [Range(typeof(decimal), MinPricePerProcedure, MaxPricePerProcedure,
            ErrorMessage = "Price Per Procedure must be a positive number and less than {2} BGN.")]

        public decimal Cost { get; set; }

        [Display(Name = "Select start date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]

        public DateTime StartDate { get; set; } = DateTime.Now;

        [Display(Name = "Select end date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]

        public DateTime EndDate { get; set; } = DateTime.Now;

        [Display(Name = "Select your patient")]

        public int PatientId { get; set; }

        public IEnumerable<PatientModel> Patients { get; set; } = new List<PatientModel>();
    }
}
