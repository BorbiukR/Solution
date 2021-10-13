using System.Collections.Generic;

namespace Solution.Contracts.Responses.Authentication
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}