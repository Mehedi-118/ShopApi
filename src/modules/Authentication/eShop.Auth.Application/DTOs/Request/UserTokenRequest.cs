using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Auth.Application.DTOs.Request
{
    public class UserTokenRequest
    {
        [Required]
        public string RefreashToken { get; set; }
    }
}
