using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Account
{
    public class RegisterDTO
    {

        public string Username { get; set; }


        public string Email { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
