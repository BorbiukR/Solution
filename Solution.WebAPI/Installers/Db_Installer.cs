using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solution.DAL;

namespace Solution.WebAPI.Installers
{
    public class Db_Installer : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            string connection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<SolulitonContext>(options => options.UseSqlServer(connection));

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 5;
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<SolulitonContext>()
              .AddDefaultTokenProviders();
        }
    }
}