using Bc = BCrypt.Net.BCrypt;


namespace Ticketing.Helpers{
    public class PasswordHelpers
{
    public static string HashPassword(string plainpassword)
    {
        return Bc.HashPassword(plainpassword);
    }

    public static bool VerifyPassword(string plainPassword, string hashedPassword)
    {
        return Bc.Verify(plainPassword, hashedPassword);
    }
}
}
