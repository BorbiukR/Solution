using Solution.Contracts.Requests.Authentication;
using Swashbuckle.AspNetCore.Filters;

namespace Solution.WebAPI.SwaggerExamples.Requests
{
    public class UserLoginRequestExamples : IExamplesProvider<UserLoginRequest>
    {
        public UserLoginRequest GetExamples()
        {
            return new UserLoginRequest
            {
                Email = "orbiuk.r@gmail.com",
                Password = "Aa/1234"
            };
        }
    }
}