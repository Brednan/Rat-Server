using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rat_Server.Model.Services
{
    public class JwtService
    {
        private readonly string jwtIssuer;
        private readonly string jwtKey;

        public JwtService(IConfiguration config)
        {
            jwtIssuer = config["JWT_ISSUER"];
            jwtKey = config["JWT_KEY"];
        }

        public string GenerateJwtToken(Claim[] claims, bool expires)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var Sectoken = expires ? new JwtSecurityToken(jwtIssuer,
                                                          jwtIssuer,
                                                          claims,
                                                          expires: DateTime.Now.AddMinutes(120),
                                                          signingCredentials: credentials)
                                   : new JwtSecurityToken(jwtIssuer,
                                                          jwtIssuer,
                                                          claims,
                                                          signingCredentials: credentials);

            string token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }

        public JwtSecurityToken DecodeJwtString(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
        }

        public string GetJwtClaimValue(string jwtToken, string claimType)
        {
            JwtSecurityToken decodedJwtToken = DecodeJwtString(jwtToken);
            return decodedJwtToken.Claims.First(c => c.Type.Equals(claimType)).Value;
        }

        /// <summary>
        /// Returns the token from the string in the Authorization header.
        /// </summary>
        /// <param name="authorizationHeader">
        ///     The header to extract the token from.
        ///     Format: Bearer <TOKEN>
        /// </param>
        /// <returns>The JWT token extracted from the string</returns>
        public string ParseAuthorizationHeader(string authorizationHeader)
        {
            return authorizationHeader.Split(' ')[1];
        }
    }
}
