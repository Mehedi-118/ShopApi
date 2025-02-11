using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Auth.Application.DTOs.Response
{
    public class UserResponse
    {
        public long UserId { get; set; }
        public string UserName { get; set; } = String.Empty;
        public string PhoneNumber { get; set; } = String.Empty;
        public string ProfilePicturePath { get; set; } = String.Empty;


        public string Email { get; set; } = String.Empty;
        public string[] Roles { get; set; } = [];
        public string[] Permissions { get; set; } = [];
        public string[] Claims { get; set; } = [];

    }
}
