using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DentalClinicApp.Infrastructure.Data.Constants.DataConstant.Prescription;

namespace DentalClinicApp.Infrastructure.Data.Entities
{
    public class Prescription
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxMedicineName)]

        public string MedicineName { get; set; } = null!;

        public int MedicineQuаtity { get; set; }

        [Required]
        [MaxLength(MaxMedicineDescription)]

        public string Description { get; set; } = null!;

        public int TreatmentId { get; set; }

        [ForeignKey(nameof(TreatmentId))]

        public Treatment Treatment { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
