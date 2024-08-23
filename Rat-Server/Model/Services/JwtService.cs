﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rat_Server.Model.Services
{
    public class JwtService
    {
        IConfiguration _config;

        public JwtService(IConfiguration config) 
        {
            _config = config;
        }

        public string GenerateJwtToken(Claim[] claims, bool expires)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT_KEY"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var Sectoken = expires ? new JwtSecurityToken(_config["JWT_ISSUER"],
                                                          _config["JWT_ISSUER"],
                                                          claims,
                                                          expires: DateTime.Now.AddMinutes(120),
                                                          signingCredentials: credentials)
                                   : new JwtSecurityToken(_config["JWT_ISSUER"],
                                                          _config["JWT_ISSUER"],
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
