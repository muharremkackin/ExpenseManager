using System.Security.Cryptography;

namespace ExpenseManager.Generators;

public static class StringGenerator
{
    public const string UppercaseAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string LowercaseAlphabet = "abcdefghijklmnopqrstuvwxyz";
    public const string Numbers = "0123456789";

    public static string Generate(int length = 6, StringType type = StringType.Alphabet)
    {
        return string.Create(length, GetCharacterSet(type), (span, characterSet) => { 
            for (int i = 0; i<span.Length; i++)
            {
                var randomIndex = RandomNumberGenerator.GetInt32(0, characterSet.Length);
                span[i] = characterSet[randomIndex];
            }
        });
    }

    private static string GetCharacterSet(StringType type)
    {
        return type switch
        {
            StringType.UppercaseAlphabet => UppercaseAlphabet,
            StringType.LowercaseAlphabet => LowercaseAlphabet,
            StringType.Alphabet => UppercaseAlphabet + LowercaseAlphabet,
            StringType.Numbers => Numbers,
            StringType.UppercaseAlphanumeric => UppercaseAlphabet + Numbers,
            StringType.LowercaseAlphanumeric => LowercaseAlphabet + Numbers,
            StringType.Alphanumeric => UppercaseAlphabet + LowercaseAlphabet + Numbers,
            _ => UppercaseAlphabet
        };
    }

    public enum StringType {
        UppercaseAlphabet,
        LowercaseAlphabet,
        Alphabet,
        Numbers,
        UppercaseAlphanumeric,
        LowercaseAlphanumeric,
        Alphanumeric
    }
}
