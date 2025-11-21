using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace ExpenseManager.Cryptography;

public static class PasswordHasher
{
    private const int SaltSize = 16;
    private const int DegreeOfParallelism = 2;
    private const int MemorySize = 65536; 
    private const int Iterations = 4;
    private const int HashSize = 16;

    public static string Hash(string password)
    {
        var salt = CreateSalt();

        using var argon = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = DegreeOfParallelism,
            MemorySize = MemorySize,
            Iterations = Iterations
        };

        var hash = argon.GetBytes(HashSize);

        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }

    public static bool Verify(string password, string passwordHash)
    {
        var parts = passwordHash.Split(":");
        if (parts.Length != 2) return false;

        var salt = Convert.FromBase64String(parts[0]);
        if (salt == null) return false;

        var hash = Convert.FromBase64String(parts[1]);
        if (hash == null) return false;

        using var argon = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = DegreeOfParallelism,
            MemorySize = MemorySize,
            Iterations = Iterations
        };

        var newHash = argon.GetBytes(HashSize);

        return newHash.SequenceEqual(hash);
    }

    public static byte[] CreateSalt()
    {
        var buffer = new byte[SaltSize];
        using var generator = RandomNumberGenerator.Create();

        generator.GetBytes(buffer);

        return buffer;
    }
}
