using DentalClinicApp.Infrastructure.Data.Entities;
using DentalClinicApp.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicApp.Infrastructure.Data
{
    public class DentalClinicDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public DentalClinicDbContext(DbContextOptions<DentalClinicDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<ApplicationUser>()
                .Property(p => p.IsActive)
                .HasDefaultValue(true);

            builder
                .Entity<Patient>()
                .HasMany(p => p.Appointments)
                .WithOne(p => p.Patient)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Patient>()
                .HasMany(p => p.Attendances)
                .WithOne(p => p.Patient)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Patient>()
                .HasMany(p => p.DentalProcedures)
                .WithOne(p => p.Patient)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }

        public DbSet<Dentist> Dentists { get; set; } = null!;

        public DbSet<Patient> Patients { get; set; } = null!;

        public DbSet<Manager> Managers { get; set; } = null!;

        public DbSet<DentalProblem> DentalProblems { get; set; } = null!;

        public DbSet<Treatment> Treatments { get; set; } = null!;

        public DbSet<Prescription> Prescriptions { get; set; } = null!;

        public DbSet<Appointment> Appointments { get; set; } = null!;

        public DbSet<DentalProcedure> DentalProcedures { get; set; } = null!;

        public DbSet<Attendance> Attendances { get; set; } = null!;


    }
}