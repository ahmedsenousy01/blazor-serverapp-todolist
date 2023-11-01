using Microsoft.AspNetCore.Identity;

namespace ServerApp.Data.Services.Security;
public static class HashingService
{
    public static string HashPassword(string password)
    {
        return new PasswordHasher<object>().HashPassword(null!, password);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        var passwordVerificationResult = new PasswordHasher<object>().VerifyHashedPassword(null!, hashedPassword, password);
        return passwordVerificationResult == PasswordVerificationResult.Success;
    }
}