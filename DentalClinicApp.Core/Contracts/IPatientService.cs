using DentalClinicApp.Core.Models.Dentists;
using DentalClinicApp.Core.Models.Patients;
using DentalClinicApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Contracts
{
	public interface IPatientService
	{
		Task CreatePatientAsync(Guid userId, int dentistId);

		Task<IEnumerable<DentistModel>> GetDentistAsync();

		Task<int> GetPatientIdAsync(Guid userId);

        Task<bool> IsExistsByIdAsync(Guid userId);

		Task<MyPatientsViewModel> GetMyPatients(Guid userId);
    }
}
