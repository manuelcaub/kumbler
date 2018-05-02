using Microsoft.IdentityModel.Tokens;

namespace Kumbler.Authentication.Models
{
    public class JwtOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecretKey { get; set; }

        public string ConnectionString { get; set; }

        public SigningCredentials SigningCredentials { get; set; }
    }
}