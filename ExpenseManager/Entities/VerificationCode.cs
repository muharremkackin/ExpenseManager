using ExpenseManager.Cryptography;
using ExpenseManager.Generators;

namespace ExpenseManager.Entities
{
    public class VerificationCode
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string Receiver { get; set; } = null!; // Email or Phone number
        public string Code { get; set; } = null!;
        public bool IsUsed { get; set; } = false;
        public int TryCount { get; set; }
        public DateTime? ExpirationDate { get; set; } = DateTime.UtcNow.AddMinutes(5);

        public static VerificationCode Create(Guid userId, string receiver, string code, DateTime? expirationDate = null)
        {
           
            return new VerificationCode
            {
                UserId = userId,
                Receiver = receiver,
                Code = code,
                ExpirationDate = expirationDate ?? DateTime.UtcNow.AddMinutes(5)
            };
        }

        public static bool Verify(VerificationCode verificationCode, string code)
        {
            if (PasswordHasher.Verify(code, verificationCode.Code))
            {
                Use(verificationCode);
                return true;
            }
            
            FailedTry(verificationCode);
            return false;
        }
        
        public static VerificationCode Use(VerificationCode verificationCode)
        {
            verificationCode.IsUsed = true;
            return verificationCode;
        }

        public static VerificationCode FailedTry(VerificationCode verificationCode)
        {
            verificationCode.TryCount += 1;
            return verificationCode;
        }

        public static (string, string) GenerateCode(int length = 6)
        {
            var code = StringGenerator.Generate(length, StringGenerator.StringType.Numbers);
            return (code, PasswordHasher.Hash(code));
        }

        public User User { get; set; } = null!;
    }
}
