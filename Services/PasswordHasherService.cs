using System;
using Bc = BCrypt.Net.BCrypt;

public class PasswordHasherService : IPasswordHasherService
{
    public string HashPassword(string plainpassword)
    {
        var salt = Bc.GenerateSalt();

        return Bc.HashPassword(plainpassword, salt);
    }

    public bool VerifyPassword(string plainPassword, string hashedPassword)
    {
        return Bc.Verify(plainPassword, hashedPassword);
    }
}