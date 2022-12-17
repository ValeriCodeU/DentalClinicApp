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
	}
}
