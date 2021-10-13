using Solution.Contracts.Requests.Authentication;
using Solution.Identity.Models;
using System.Threading.Tasks;

namespace Solution.Identity.Interfaces
{
    public interface IUserService
    {
        Task<AuthenticationResult> RegisterUserAsync(UserRegistrationRequest model);

        Task<AuthenticationResult> LoginUserAsync(UserLoginRequest model);
    }
}