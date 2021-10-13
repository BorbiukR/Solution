using System;

namespace Solution.Identity.Settings
{
    public class JwtSettings
    {
        public string Secret { get; set; }

        public TimeSpan TokenLifetime { get; set; }
    }
}