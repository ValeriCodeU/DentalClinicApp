namespace DentalClinicApp.Core.Models.DentalProcedures
{
    public class ProcedureQueryServiceModel
	{
        public int TotalProceduresCount { get; set; }

        public IEnumerable<ProcedureServiceModel> Procedures { get; set; } = new List<ProcedureServiceModel>();
    }
}
