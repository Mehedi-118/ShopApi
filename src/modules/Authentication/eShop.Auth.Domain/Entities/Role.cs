using System.Reflection.Metadata.Ecma335;

using Microsoft.AspNetCore.Identity;

namespace eShop.Auth.Domain.Entities;

public class Role : IdentityRole<long>
{
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

}