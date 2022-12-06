using DentalClinicApp.Core.Models.DentalProblems;
using DentalClinicApp.Core.Models.Patients;
using DentalClinicApp.Infrastructure.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Contracts
{
    public interface IProblemService
    {
        Task CreateAsync(int patientId, ProblemFormModel model);
    }
}