using ExpenseManager.Cryptography;

namespace ExpenseManager.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    public static User Create(string name, string lastName, string email)
    {
        return new User { 
            Name = name,
            LastName = lastName,
            Email = email
        };
    }

    public static User SetPassword(User user, string password)
    {
        user.Password = PasswordHasher.Hash(password);
        return user;
    }

    public static bool VerifyPassword(User user, string password) {
        return PasswordHasher.Verify(password, user.Password);
    }


    public ICollection<VerificationCode> VerificationCodes { get; set; } = [];
}
