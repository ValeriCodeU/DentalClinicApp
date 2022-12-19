using DentalClinicApp.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentalClinicApp.Infrastructure.Configuration
{
    internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasData(CollectionOfUsers());
        }

        private List<ApplicationUser> CollectionOfUsers()
        {
            var users = new List<ApplicationUser>();

            var hasher = new PasswordHasher<ApplicationUser>();

            //Admin user and manager of the clinic

            var adminUser = new ApplicationUser()
            {
                Id = new Guid("48787569-f841-4832-8528-1f503a8427cf"),
                FirstName = "Lionel",
                LastName = "Scaloni",
                UserName = "lionel",
                NormalizedUserName = "LIONEL",
                Email = "lionel@mail.com",
                NormalizedEmail = "LIONEL@MAIL.COM",
                PhoneNumber = "222222222222222",
            };

            adminUser.PasswordHash =
                 hasher.HashPassword(adminUser, "lionel123");

            users.Add(adminUser);

            //Dentist users

            var dentistGeorgi = new ApplicationUser()
            {
                Id = new Guid("bfbcc7d7-2e7e-4d3c-b7fb-4b76f27cefe3"),
                FirstName = "Georgi",
                LastName = "Ivanov",
                UserName = "georgi",
                NormalizedUserName = "GEORGI",
                Email = "georgi@mail.com",
                NormalizedEmail = "GEORGI@MAIL.COM",
                PhoneNumber = "000000000000",
            };

            dentistGeorgi.PasswordHash =
                 hasher.HashPassword(dentistGeorgi, "georgi123");

            users.Add(dentistGeorgi);

            var dentistGencho = new ApplicationUser()
            {
                Id = new Guid("94a79c1d-5a55-4260-815a-d5b827d93a1d"),
                FirstName = "Gencho",
                LastName = "Genchev",
                UserName = "gencho",
                NormalizedUserName = "GENCHO",
                Email = "gencho@mail.com",
                NormalizedEmail = "GENCHO@MAIL.COM",
                PhoneNumber = "9999999999999",
            };

            dentistGencho.PasswordHash =
                 hasher.HashPassword(dentistGencho, "gencho123");

            users.Add(dentistGencho);

            //Patient user

            var userDimitar = new ApplicationUser()
            {
                Id = new Guid("da24feae-ab42-4702-bbf9-9c5361aee8d6"),
                FirstName = "Dimitar",
                LastName = "Georgiev",
                UserName = "dimitar",
                NormalizedUserName = "DIMITAR",
                Email = "dimitar@mail.com",
                NormalizedEmail = "DIMITAR@MAIL.COM",
                PhoneNumber = "1111111111111",
            };

            userDimitar.PasswordHash =
                 hasher.HashPassword(userDimitar, "dimitar123");

            users.Add(userDimitar);

            //Simple user to become a patient

            var userVasil = new ApplicationUser()
            {
                Id = new Guid("e28afed9-0de3-4ca6-aee8-28488401bca8"),
                FirstName = "Vasil",
                LastName = "Georgiev",
                UserName = "vasil",
                NormalizedUserName = "VASIL",
                Email = "vasil@mail.com",
                NormalizedEmail = "VASIL@MAIL.COM",
                PhoneNumber = "33333333333333",
            };

            userVasil.PasswordHash =
                 hasher.HashPassword(userVasil, "vasil123");

            users.Add(userVasil);


            return users;
        }
    }
}
