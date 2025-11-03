namespace AuthAPI.Helpers;

using System.Security.Cryptography;
using System;

public static class PasswordHasher
{
    private const int SaltSize = 16;     // 128 bit
    private const int HashSize = 20;     // 160 bit
    private const int Iterations = 10000;

    public static string HashPassword(string password)
    {
        // Generate Salt
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        // Derive Key
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);

        byte[] hash = pbkdf2.GetBytes(HashSize);

        // Combine salt and hash
        byte[] hashBytes = new byte[SaltSize + HashSize];
        Buffer.BlockCopy(salt, 0, hashBytes, 0, SaltSize);
        Buffer.BlockCopy(hash, 0, hashBytes, SaltSize, HashSize);

        return Convert.ToBase64String(hashBytes);
    }

    public static bool VerifyPassword(string password, string storedHash)
    {
        // Extract bytes
        byte[] hashBytes = Convert.FromBase64String(storedHash);

        byte[] salt = new byte[SaltSize];
        Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);

        byte[] storedSubKey = new byte[HashSize];
        Buffer.BlockCopy(hashBytes, SaltSize, storedSubKey, 0, HashSize);

        // Hash input password with same salt
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        byte[] computedSubKey = pbkdf2.GetBytes(HashSize);

        // Compare byte-by-byte (time-constant)
        return CryptographicOperations.FixedTimeEquals(storedSubKey, computedSubKey);
    }
}

// private static readonly int _saltSize = 16;
// private static readonly int _hashSize = 32;
// private static readonly int _iterations = 100000;
// public static string HashPassword(string password)
// {
//     byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);
//     var key = new Rfc2898DeriveBytes(password, salt, _iterations, HashAlgorithmName.SHA256);

//     byte[] hash = key.GetBytes(_hashSize);

//     return Convert.ToBase64String(salt) + "|" + Convert.ToBase64String(hash);
// }

// public static bool VerifyPassword(string password, string storedHash)
// {
//     var parts = storedHash.Split('|');
//     byte[] salt = Convert.FromBase64String(parts[0]);
//     byte[] storedHashBytes = Convert.FromBase64String(parts[1]);

//     var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
//     byte[] computedHash = pbkdf2.GetBytes(32);

//     return CryptographicOperations.FixedTimeEquals(computedHash, storedHashBytes);
// }
// }
