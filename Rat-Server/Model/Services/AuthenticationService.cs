using Microsoft.AspNetCore.Identity;

namespace Rat_Server.Model.Services
{
    public class AuthenticationService
    {
        private readonly IPasswordHasher<string> _passwordHasher;

        public AuthenticationService()
        {
            _passwordHasher = new PasswordHasher<string>();
        }

        /// <summary>
        /// Compares a unhashed password with it's hashed counterpart.
        /// Normally used for verifying a user's entered password with
        /// the hashed password stored in the Database.
        /// </summary>
        /// <param name="hashedPassword"></param>
        /// <param name="providedPassword"></param>
        /// <returns></returns>
        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var verificationResult = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return verificationResult == PasswordVerificationResult.Success;
        }
    }
}
