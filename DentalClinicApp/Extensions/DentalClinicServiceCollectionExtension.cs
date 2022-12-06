using DentalClinicApp.Core.Contracts;
using DentalClinicApp.Core.Services;
using DentalClinicApp.Infrastructure.Data.Common;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DentalClinicServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProblemService, ProblemService>();
            services.AddScoped<IPatientService, PatientService>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            });

            return services;
        }
    }
}
