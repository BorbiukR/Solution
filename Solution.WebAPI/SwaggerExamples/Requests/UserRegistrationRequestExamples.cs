using Solution.Contracts.Requests.Authentication;
using Swashbuckle.AspNetCore.Filters;

namespace Solution.WebAPI.SwaggerExamples.Requests
{
    public class UserRegistrationRequestExamples : IExamplesProvider<UserRegistrationRequest>
    {
        public UserRegistrationRequest GetExamples()
        {
            return new UserRegistrationRequest
            {
                Email = "simplekek@gmail.com",
                Password = "Aa/1234",
                ConfirmPassword = "Aa/1234"
            };
        }
    }
}