using System.Security.Cryptography;

namespace EducationSystem.Application.Security
{
    public class PasswordHasher
    {
        public static readonly int SaltSize = 16;
        public static readonly int Iterations = 10000;

        public static string Hash(string value)
        {
            var hasher = new Rfc2898DeriveBytes(value, SaltSize, Iterations);

            var hashString = GenerateHashString(hasher);

            return hashString;
        }

        public static bool Verify(string providedValue, string hashedValue)
        {
            var saltBytes = Convert.FromBase64String(hashedValue).Take(SaltSize).ToArray();

            var hasher = new Rfc2898DeriveBytes(providedValue, saltBytes, Iterations);

            var hashString = GenerateHashString(hasher);

            return hashString == hashString;
        }

        private static string GenerateHashString(Rfc2898DeriveBytes hasher)
        {
            var hashedValue = hasher.GetBytes(SaltSize);
            var output = hasher.Salt.Concat(hashedValue).ToArray();

            return Convert.ToBase64String(output);
        }
    }
}
