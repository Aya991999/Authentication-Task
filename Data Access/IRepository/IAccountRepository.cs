using Models.DTO.Account;
using Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.IRepository
{
    public interface IAccountRepository
    {
         Task<ResponseAccount> Login(LoginDTO loginDTO);

        Task<ResponseAccount> Registration(RegisterDTO registerDTO);
        Task<ResponseAccount> ResetPassword(ResetPasswordDTO resetPasswordDTO);
        Task<ResponseAccount> ForgetPassword(string email);
    }
}
