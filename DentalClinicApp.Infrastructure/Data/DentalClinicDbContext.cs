using DentalClinicApp.Infrastructure.Configuration;
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


            builder
                .Entity<Dentist>()
                .HasMany(d => d.Appointments)
                .WithOne(d => d.Dentist)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Dentist>()
                .HasMany(d => d.Attendances)
                .WithOne(d => d.Dentist)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Dentist>()
                .HasMany(d => d.DentalProcedures)
                .WithOne(d => d.Dentist)
                .OnDelete(DeleteBehavior.Restrict);

            builder
               .Entity<Dentist>()
               .HasMany(d => d.Patients)
               .WithOne(d => d.Dentist)
               .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Manager>()
                .HasMany(m => m.AcceptedDentists)
                .WithOne(m => m.Manager)
                .OnDelete(DeleteBehavior.Restrict);


            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new ManagerConfiguration());
            builder.ApplyConfiguration(new DentistConfiguration());
            builder.ApplyConfiguration(new PatientConfiguration());
            builder.ApplyConfiguration(new ProblemConfiguration());
            builder.ApplyConfiguration(new AttendanceConfiguration());
            builder.ApplyConfiguration(new ProcedureConfiguration());
            builder.ApplyConfiguration(new AppointmentConfiguration());



            this.SeedRoles(builder);
            this.SeedUserRoles(builder);

            base.OnModelCreating(builder);
        }

        public DbSet<Dentist> Dentists { get; set; } = null!;

        public DbSet<Patient> Patients { get; set; } = null!;

        public DbSet<Manager> Managers { get; set; } = null!;

        public DbSet<DentalProblem> DentalProblems { get; set; } = null!;

        public DbSet<Appointment> Appointments { get; set; } = null!;

        public DbSet<DentalProcedure> DentalProcedures { get; set; } = null!;

        public DbSet<Attendance> Attendances { get; set; } = null!;

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid>() { Id = new Guid("b3bf89bc-b80a-4b24-9f4b-9a90bb4fe652"), Name = "Administrator", ConcurrencyStamp = "1", NormalizedName = "ADMINISTRATOR" },
                new IdentityRole<Guid>() { Id = new Guid("5fb4a609-b894-4db7-a7cf-7fe80cb6d536"), Name = "Dentist", ConcurrencyStamp = "2", NormalizedName = "DENTIST" },
                new IdentityRole<Guid>() { Id = new Guid("011435d5-c975-4282-99f6-5f444cea0ad6"), Name = "Patient", ConcurrencyStamp = "3", NormalizedName = "PATIENT" },
                new IdentityRole<Guid>() { Id = new Guid("eceb86a7-7d98-4d23-b00d-ac31447e3de0"), Name = "User", ConcurrencyStamp = "4", NormalizedName = "USER" }
                );
        }

        private void SeedUserRoles(ModelBuilder builder)
        {
            //Admin

            builder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>() { RoleId = new Guid("b3bf89bc-b80a-4b24-9f4b-9a90bb4fe652"), UserId = new Guid("48787569-f841-4832-8528-1f503a8427cf") }
                );

            //Dentists

            builder.Entity<IdentityUserRole<Guid>>().HasData(
               new IdentityUserRole<Guid>() { RoleId = new Guid("5fb4a609-b894-4db7-a7cf-7fe80cb6d536"), UserId = new Guid("bfbcc7d7-2e7e-4d3c-b7fb-4b76f27cefe3") }
               );

            builder.Entity<IdentityUserRole<Guid>>().HasData(
              new IdentityUserRole<Guid>() { RoleId = new Guid("5fb4a609-b894-4db7-a7cf-7fe80cb6d536"), UserId = new Guid("94a79c1d-5a55-4260-815a-d5b827d93a1d") }
              );

            //Patient

            builder.Entity<IdentityUserRole<Guid>>().HasData(
               new IdentityUserRole<Guid>() { RoleId = new Guid("011435d5-c975-4282-99f6-5f444cea0ad6"), UserId = new Guid("da24feae-ab42-4702-bbf9-9c5361aee8d6") }
               );


            //User

            builder.Entity<IdentityUserRole<Guid>>().HasData(
               new IdentityUserRole<Guid>() { RoleId = new Guid("eceb86a7-7d98-4d23-b00d-ac31447e3de0"), UserId = new Guid("e28afed9-0de3-4ca6-aee8-28488401bca8") }
               );
        }
    }
}