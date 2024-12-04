using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;
using System.Text.RegularExpressions;

using eShop.Auth.Domain.Entities.MetaInfo;

using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

namespace eShop.Auth.Domain.Entities;

public partial class User : IdentityUser<long>, ICreationMetadata, IModificationMetadata, IEntityStatus
{
    public string ProfilePicturePath { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public long? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastLogin { get; init; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }

    [NotMapped] public Dictionary<string, List<string>> ValidationErrors { get; private init; }

    public static UserBuilder CreateBuilder() => new();

    private User() { }

    public class UserBuilder
    {
        private string _userName = string.Empty;
        private string _email = string.Empty;
        private string _phoneNumber = string.Empty;
        private string _password = string.Empty;
        private DateTime? _lastLogin;
        private readonly Dictionary<string, List<string>> _validationErrors = new();

        public UserBuilder WithUserName(string userName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                {
                    _validationErrors.TryAdd("User Name", new List<string> { "Username is required" });
                }
                else
                {
                    _userName = Strings.Trim(userName);
                }

                return this;
            }
            catch (Exception e)
            {
                _validationErrors.TryAdd("User Name ", ["Failed to Get Username "]);
                return this;
            }
        }

        public UserBuilder WithEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    _validationErrors.TryAdd("Email", new List<string> { "Email is required" });
                }

                if (!IsEmailValid(email))
                {
                    _validationErrors.TryAdd("Email", new List<string> { "Email is invalid" });
                }

                else
                {
                    _email = email;
                }

                return this;
            }
            catch (Exception e)
            {
                _validationErrors.TryAdd("Email", ["Failed to get email"]);
                return this;
            }
        }

        public UserBuilder WithPhoneNumber(string phoneNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    _validationErrors.TryAdd("PhoneNumber", ["Phone number is required"]);
                }


                if (!IsPhoneNumberValid(phoneNumber))
                {
                    _validationErrors.TryAdd("PhoneNumber",
                        ["Phone number should be digit only & Lenght should be 11"]);
                }
                else
                {
                    _phoneNumber = phoneNumber;
                }

                return this;
            }
            catch (Exception e)
            {
                _validationErrors.TryAdd("PhoneNumber", ["Failed to get phone number"]);
                return this;
            }
        }

        public UserBuilder WithPasswordHash(string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    _validationErrors.TryAdd("Password", ["Password is required"]);
                }

                var result = PasswordValidator(password);
                if (!result.isValid)
                {
                    _validationErrors.TryAdd("Password", PasswordValidator(password).errors);
                }
                else
                {
                    _password = password;
                }

                return this;
            }
            catch (Exception e)
            {
                _validationErrors.TryAdd("Password", ["Failed to get password"]);
                return this;
            }
        }

        public UserBuilder SetLastLogIn()
        {
            _lastLogin = DateTime.Now;
            return this;
        }

        public UserBuilder WithLogInCredential(string userName, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                {
                    _validationErrors.TryAdd("UserName", ["Username is required"]);
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    _validationErrors.TryAdd("Password", ["Password is required"]);
                }

                _userName = userName;
                _password = password;

                return this;
            }
            catch (Exception e)
            {
                _validationErrors.TryAdd("LogInCredential", ["Failed to get login credential"]);
                return this;
            }
        }

        public User Build()
        {
            return new User
            {
                UserName = _userName,
                Email = _email,
                PhoneNumber = _phoneNumber,
                PasswordHash = _password,
                ProfilePicturePath = string.Empty,
                LastLogin = _lastLogin,
                ValidationErrors = _validationErrors
            };
        }
    }

    private static bool IsEmailValid(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsPhoneNumberValid(string phoneNumber)
    {
        return PhoneNumberValidatorRegex().Match(phoneNumber).Success;
    }

    public void Update(User user)
    {
        if (string.IsNullOrWhiteSpace(user.UserName))
        {
            user.UserName = user.Email;
        }

        UserName = user.UserName;
        Email = user.Email;
        PhoneNumber = user.PhoneNumber;
        UpdatedAt = DateTime.Now;
    }

    public void SetEmailConfirmed()
    {
        base.EmailConfirmed = true;
    }

    public void SetPhoneNumberConfirmed()
    {
        base.PhoneNumberConfirmed = true;
    }

    public void SetTwoFactorEnabled()
    {
        base.TwoFactorEnabled = true;
    }

    public void SetLockoutEnabled()
    {
        base.LockoutEnabled = true;
    }

    public void SetLockoutEndDate(DateTimeOffset? lockoutEndDate)
    {
        base.LockoutEnd = lockoutEndDate;
    }

    private static (bool isValid, List<string>errors) PasswordValidator(string passwordHash)
    {
        var errors = new List<string>();
        if (passwordHash.Length < 8)
        {
            errors.Add("Password should be at least 8 characters long");
        }

        if (!passwordHash.Any(char.IsUpper))
        {
            errors.Add("Password should contain at least one uppercase letter");
        }

        if (!passwordHash.Any(char.IsLower))
        {
            errors.Add("Password should contain at least one lowercase letter");
        }

        if (!passwordHash.Any(char.IsDigit))
        {
            errors.Add("Password should contain at least one digit");
        }

        if (passwordHash.All(char.IsLetterOrDigit))
        {
            errors.Add("Password should contain at least one special character");
        }

        return errors.Count > 0 ? (false, errors) : (true, errors);
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void UpdateFailedLoginCount()
    {
        base.AccessFailedCount++;
    }


    [GeneratedRegex(@"^(88|\+88)?\d{11}$")]
    private static partial Regex PhoneNumberValidatorRegex();

    /*[GeneratedRegex(@"/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[a-zA-Z\d@$!%*?&#]{8,}$/")]
    private static partial Regex PasswordValidatorRegex();*/
}