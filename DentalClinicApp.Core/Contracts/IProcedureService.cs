using DentalClinicApp.Core.Models.DentalProcedures;
using DentalClinicApp.Core.Models.DentalProcedures.Enums;

namespace DentalClinicApp.Core.Contracts
{
    public interface IProcedureService
	{
		Task<int> CreateAsync(ProcedureFormModel model, int dentistId);

		Task<bool> ProcedureExistsAsync(int id);

        Task<ProcedureServiceModel> ProcedureDetailsByIdAsync(int id);

        Task<ProcedureQueryServiceModel> GetDentistProceduresAsync(
            int dentistId,
            ProcedureSorting sorting = ProcedureSorting.StartDate,
            string? searchTerm = null,
            int currentPage = 1,
            int proceduresPerPage = 1
            );


        Task EditProcedureAsync(ProcedureFormModel model, int procedureId);

        Task<IEnumerable<ProcedureDetailsViewModel>> AllProceduresByPatientIdAsync(int patientId);

		Task<bool> DeleteProcedureAsync(int id);
    }
}
