using System.Security.Cryptography;

namespace JadedCmsCore.Services.Core;

public class PasswordEncryption
{
    private const int SaltSize = 16; // 128 bit 
    private const int KeySize = 32; // 256 bit
    private const int Iterations = 10000; // Number of iterations for PBKDF2

    public string HashPassword(string password)
    {
        using (var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA256))
        {
            var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
            var salt = Convert.ToBase64String(algorithm.Salt);

            return $"{Iterations}.{salt}.{key}";
        }
    }

    public bool VerifyPassword(string hash, string password)
    {
        var parts = hash.Split('.', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 3)
        {
            throw new FormatException("Unexpected hash format. Should be formatted as `{iterations}.{salt}.{hash}`");
        }

        var iterations = Convert.ToInt32(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);
        var key = Convert.FromBase64String(parts[2]);

        using (var algorithm = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
        {
            var keyToCheck = algorithm.GetBytes(KeySize);

            return keyToCheck.SequenceEqual(key);
        }
    }
}