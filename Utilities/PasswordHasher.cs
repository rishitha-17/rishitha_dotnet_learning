using System.Security.Cryptography;
using System.Text;

namespace policy_management.Utilities
{
    public static class PasswordHasher
    {
        private const int SaltSize = 128 / 8;
        private const int KeySize = 256 / 8; 
        private const int Iterations = 10000;
        private const char Delimiter = ';';

        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));

            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);
            
            return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public static bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordHash))
                return false;

            try
            {
                var elements = passwordHash.Split(Delimiter);
                if (elements.Length != 2)
                    return false;

                var salt = Convert.FromBase64String(elements[0]);
                var hash = Convert.FromBase64String(elements[1]);
                
                var hashInput = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);
                
                return CryptographicOperations.FixedTimeEquals(hash, hashInput);

                // return passwordHash == password;
            }
            catch
            {
                return false;
            }
        }
    }
}