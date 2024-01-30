public interface IPasswordHasherService
{
      string HashPassword(string plainpassword);
      bool VerifyPassword(string plainPassword, string hashedPassword);
}