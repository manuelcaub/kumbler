using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Kumbler.Authentication.Models
{
    public class JwtFactory
    {
        private readonly JwtOptions jwtOptions;
        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;

        public JwtFactory(IOptions<JwtOptions> jwtOptions, JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            this.jwtOptions = jwtOptions.Value;
            this.jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }

        public string GenerateEncodedToken(string userName)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var jwt = new JwtSecurityToken(
                issuer: this.jwtOptions.Issuer,
                audience: this.jwtOptions.Audience,
                claims: claims,
                signingCredentials: this.jwtOptions.SigningCredentials);

            return this.jwtSecurityTokenHandler.WriteToken(jwt);
        }
    }
}