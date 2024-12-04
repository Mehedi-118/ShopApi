using System.Text.RegularExpressions;

namespace eShop.Auth.Helpers;

public class ValidationChecker
{
    public static bool IsEmailValid(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsPasswordValid(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        if (password.Length < 6)
            return false;
        
        // Regex to check for at least one uppercase letter
        var upperCaseRegex = new Regex(@"[A-Z]");
        if (!upperCaseRegex.IsMatch(password))
        {
            return false;
        }

        // Regex to check for at least one lowercase letter
        var lowerCaseRegex = new Regex(@"[a-z]");
        if (!lowerCaseRegex.IsMatch(password))
        {
            return false;
        }

        // Regex to check for at least one digit
        var digitRegex = new Regex(@"\d");
        if (!digitRegex.IsMatch(password))
        {
            return false;
        }

        // Regex to check for at least one special character (non-alphanumeric)
        var specialCharRegex = new Regex(@"[\W_]");
        if (!specialCharRegex.IsMatch(password))
        {
            return false;
        }

        return true;
    }

    public static bool IsPhoneNumberValid(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return false;
        }

        return phoneNumber.Length >= 10;
    }
}