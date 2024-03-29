﻿using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Models.Attendances;
using DentalClinicApp.Core.Models.DentalProcedures;
using DentalClinicApp.Core.Models.DentalProcedures.Enums;
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

        public async Task<IEnumerable<ProcedureDetailsViewModel>> AllProceduresByPatientIdAsync(int patientId)
        {
            return await repo.AllReadonly<Patient>()
                .Where(p => p.Id == patientId && p.User.IsActive)
                .Select(p => p.DentalProcedures
                .Where(pp => pp.IsActive)
                .Select(pp => new ProcedureDetailsViewModel()
                {
                    Name = pp.Name,
                    Description = pp.Description,
                    StartDate = pp.StartDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    EndDate = pp.EndDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Cost = decimal.Parse(pp.Cost.ToString("F2")),
                    Id = pp.Id

                })).FirstAsync();
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

        public async Task<bool> DeleteProcedureAsync(int id)
        {
            var procedure = await repo.GetByIdAsync<DentalProcedure>(id);
            procedure.IsActive = false;
            await repo.SaveChangesAsync();

            return true;
        }

        public async Task EditProcedureAsync(ProcedureFormModel model, int procedureId)
        {
            var procedure = await repo.GetByIdAsync<DentalProcedure>(procedureId);

            procedure.Name = model.Name;
            procedure.Description = model.Description;
            procedure.Cost = model.Cost;
            procedure.StartDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            procedure.EndDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            procedure.PatientId = model.PatientId;

            await repo.SaveChangesAsync();
        }

        public async Task<ProcedureQueryServiceModel> GetProceduresAsync(
            int clientId,
            bool isPatient,
            ProcedureSorting sorting = ProcedureSorting.StartDate,
            string? searchTerm = null,
            int currentPage = 1,
            int proceduresPerPage = 1
           )
        {
            var result = new ProcedureQueryServiceModel();

            IQueryable<DentalProcedure> procedures = repo.AllReadonly<DentalProcedure>();

            if (isPatient)
            {
                procedures = repo.AllReadonly<DentalProcedure>()
                .Where(a => a.PatientId == clientId && a.IsActive);
            }
            else
            {
                procedures = repo.AllReadonly<DentalProcedure>()
                .Where(a => a.DentistId == clientId && a.IsActive);
            }

            if (!String.IsNullOrEmpty(searchTerm))
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                procedures = procedures
                    .Where(p => EF.Functions.Like(p.Name.ToLower(), searchTerm) ||
                    EF.Functions.Like(p.Description.ToLower(), searchTerm));
            }

            procedures = sorting switch
            {
                ProcedureSorting.Name => procedures.OrderBy(p => p.Name),
                ProcedureSorting.StartDate => procedures.OrderBy(p => p.StartDate),
                ProcedureSorting.EndDate => procedures.OrderBy(p => p.EndDate),
                ProcedureSorting.LowestPrice => procedures.OrderBy(p => p.Cost),
                ProcedureSorting.HighPrice => procedures.OrderByDescending(p => p.Cost),
                _ => procedures.OrderByDescending(a => a.Id)
            };

            var attendanceList = await procedures.Skip((currentPage - 1) * proceduresPerPage).Take(proceduresPerPage)
                .Select(p => new ProcedureServiceModel()
                {
                    Name = p.Name,
                    Description = p.Description,
                    Id = p.Id,
                    Cost = decimal.Parse(p.Cost.ToString("F2")),
                    StartDate = p.StartDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    EndDate = p.EndDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Patient = new Models.Patients.PatientServiceModel()
                    {
                        FirstName = p.Patient.User.FirstName,
                        LastName = p.Patient.User.LastName,
                        Email = p.Patient.User.Email,
                        PhoneNumber = p.Patient.User.PhoneNumber
                    }
                }).ToListAsync();

            result.TotalProceduresCount = await procedures.CountAsync();
            result.Procedures = attendanceList;

            return result;
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
                    Cost = decimal.Parse(p.Cost.ToString("F2")),
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
