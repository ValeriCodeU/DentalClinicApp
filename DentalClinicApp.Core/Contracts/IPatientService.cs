﻿using DentalClinicApp.Core.Models.Dentists;
using DentalClinicApp.Core.Models.Patients;
using DentalClinicApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApp.Core.Contracts
{
    /// <summary>
    ///  Manipulates patient data
    /// </summary>
    public interface IPatientService
    {
        /// <summary>
        /// Create a user as a patient
        /// </summary>
        /// <param name="userId">User globally unique identifier</param>
        /// <param name="dentistId">Dentist identifier</param>
        /// <returns></returns>
        Task CreatePatientAsync(Guid userId, int dentistId);

        /// <summary>
        /// Get all dentists in the system
        /// </summary>
        /// <returns>List of dentists</returns>
        Task<IEnumerable<DentistModel>> GetDentistsAsync();

        /// <summary>
        /// Get a patient Id
        /// </summary>
        /// <param name="userId">User globally unique identifier</param>
        /// <returns>Integer identifier for a patient</returns>
        Task<int> GetPatientIdAsync(Guid userId);

        /// <summary>
        /// Check if the user exists as a patient
        /// </summary>
        /// <param name="userId">User globally unique identifier</param>
        /// <returns>Boolean data type if the user exists as a patient</returns>
        Task<bool> IsExistsByIdAsync(Guid userId);

        /// <summary>
        /// Get the dentist's patients
        /// </summary>
        /// <param name="userId">User globally unique identifier</param>
        /// <returns>Data view model for list of patients</returns>
        Task<MyPatientsViewModel> GetMyPatientsAsync(Guid userId);

        /// <summary>
        /// Get data for the patient's dental problems
        /// </summary>
        /// <param name="id">Patient identifier</param>
        /// <returns>Data view model for lists of patient data</returns>
        Task<PatientDetailsViewModel> PatientDetailsByIdAsync(int id);

        /// <summary>
        /// Get data for the patient's dental attendances
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Data view model for lists of patient data</returns>
        Task<PatientDetailsViewModel> PatientAttendanceDetailsByIdAsync(int id);

        /// <summary>
        /// Get data for the patient's dental procedures
        /// </summary>
        /// <param name="id">Patient identifier</param>
        /// <returns>Data view model for lists of patient data</returns>
        Task<PatientDetailsViewModel> PatientProcedureDetailsByIdAsync(int id);

        /// <summary>
        /// Get the Id for the patient's personal dentist
        /// </summary>
        /// <param name="patientId">Patient identifier</param>
        /// <returns>Integer identifier for a dentist</returns>
        Task<int> GetPersonalDentistIdAsync(int patientId);

        /// <summary>
        /// Get dentist patient data 
        /// </summary>
        /// <param name="userId">User globally unique identifier</param>
        /// <returns>List of patient data</returns>
        Task<IEnumerable<PatientModel>> GetPatientsAsync(Guid userId);

        /// <summary>
        /// Get user unique identifier by patient identifier
        /// </summary>
        /// <param name="id">Patient identifier</param>
        /// <returns>User globally unique identifier</returns>
        Task<Guid> GetUserIdByPatientId(int id);
    }
}