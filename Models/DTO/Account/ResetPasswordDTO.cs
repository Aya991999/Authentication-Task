using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Account
{
    public class ResetPasswordDTO
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set;}
    }
}
