using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Solution.WebAPI.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}