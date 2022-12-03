using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DentalClinicApp.Infrastructure.Data.Constants.DataConstant.Apppointment;

namespace DentalClinicApp.Infrastructure.Data.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        public DateTime StartDateTime { get; set; }

        [MaxLength(MaxDetailsLength)]

        public string? Details { get; set; }

        public bool Status { get; set; }

        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]

        public Patient Patient { get; set; } = null!;

        public int DentistId { get; set; }

        [ForeignKey(nameof(DentistId))]

        public Dentist Dentist { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
