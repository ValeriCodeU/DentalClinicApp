using DentalClinicApp.Core.Models.Attendances;
using DentalClinicApp.Core.Models.DentalProcedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Contracts
{
	public interface IProcedureService
	{
		Task<int> CreateAsync(ProcedureFormModel model, int dentistId);

		Task<bool> ProcedureExistsAsync(int id);

        Task<ProcedureServiceModel> ProcedureDetailsByIdAsync(int id);

		Task<IEnumerable<ProcedureServiceModel>> GetDentistProceduresAsync(Guid userId);

        Task EditProcedureAsync(ProcedureFormModel model, int procedureId);

        Task<IEnumerable<ProcedureDetailsViewModel>> AllProceduresByPatientIdAsync(int patientId);

		Task<bool> DeleteProcedureAsync(int id);
    }
}
