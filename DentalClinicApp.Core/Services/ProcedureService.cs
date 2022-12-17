using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Attendances;
using DentalClinicApp.Core.Models.DentalProcedures;
using DentalClinicApp.Infrastructure.Data.Common;
using DentalClinicApp.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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

		public async Task<IEnumerable<ProcedureServiceModel>> GetDentistProceduresAsync(Guid userId)
		{
            return await repo.AllReadonly<Dentist>()
                .Where(d => d.UserId == userId)
                .Where(d => d.User.IsActive)
                .Select(d => d.DentalProcedures
                .Where(d => d.IsActive)
                .Select(p => new ProcedureServiceModel()
                {
                    Name = p.Name,
                    Description= p.Description,
                    Id = p.Id,
					Cost = p.Cost,
                    StartDate = p.EndDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    EndDate = p.EndDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Patient = new Models.Patients.PatientServiceModel()
                    {
                        FirstName = p.Patient.User.FirstName,
                        LastName = p.Patient.User.LastName,
                        Email = p.Patient.User.Email,
                        PhoneNumber = p.Patient.User.PhoneNumber
                    }

                })).FirstAsync();
        }

		public async Task<ProcedureServiceModel> ProcedureDetailsByIdAsync(int id)
		{
			return await repo.AllReadonly<DentalProcedure>()
				.Where(p => p.Id == id)
				.Where(p => p.IsActive)
				.Select(p => new ProcedureServiceModel()
				{
					Id = p.Id,
					Name = p.Name,
					Description = p.Description,
					StartDate = p.StartDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
					EndDate = p.EndDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
					Cost = p.Cost,
					Patient = new Models.Patients.PatientServiceModel()
					{
						Id = p.Patient.Id,
						FirstName = p.Patient.User.FirstName,
						LastName = p.Patient.User.LastName,
						Email = p.Patient.User.Email,
						PhoneNumber = p.Patient.User.PhoneNumber
					}

				}).FirstAsync();
		}

		public async Task<bool> ProcedureExistsAsync(int id)
        {
            return await repo.AllReadonly<DentalProcedure>().AnyAsync(a => a.Id == id && a.IsActive);
        }
    }
}
