using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.DentalProcedures;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;

namespace DentalClinicApp.Core.Services
{
	public class ProcedureService : IProcedureService
	{
		private readonly IRepository repo;

		public ProcedureService(IRepository _repo)
		{
			repo = _repo;
		}

		public async Task<int> CreateAsync(ProcedureFormModel model, int dentistId)
		{
			var procedure = new DentalProcedure()
			{
				Name = model.Name,
				Description = model.Description,
				Cost = model.Cost,
				StartDate = DateTime.Parse(model.StartDate),
				EndDate = DateTime.Parse(model.EndDate),
				DentistId = dentistId,
				PatientId = model.PatientId				
			};

			await repo.AddAsync<DentalProcedure>(procedure);
			await repo.SaveChangesAsync();

			return procedure.Id;

		}
	}
}
