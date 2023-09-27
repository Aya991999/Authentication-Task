using Data_Access.UnitOfWork.AcountUnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.DbModels;
using Models.DTO.Account;
using Models.Helpers;
using Models.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Web;

namespace McShippers_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWorkAccount _unitOfWork;

  
    
        private readonly IEmailSender _emailSender;
       
        public AccountController(IUnitOfWorkAccount unitOfWork,IEmailSender emailSender)
        {
            _unitOfWork= unitOfWork;
          
            _emailSender= emailSender;
        }
        [HttpPost]
        [Route("login")]
        public async Task<ResponseAccount> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                if(loginDTO == null)
                {
                    return new ResponseAccount { Status = "Error", Message = "Enter Your Data!" };

                }
                return await _unitOfWork.accountRepository.Login(loginDTO);
            }
            catch 
            {
                return new ResponseAccount { Status = "Error", Message = "Internal Error" };

            }
        }

        [HttpPost]
        [Route("RegistorUser")]
        public async Task<ResponseAccount> Register([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                if (registerDTO == null)
                {
                    return new ResponseAccount { Status = "Error", Message = "Enter Your Data!" };

                }
                return await _unitOfWork.accountRepository.Registration(registerDTO);
            }
            catch
            {
                return new ResponseAccount { Status = "Error", Message = "Internal Error" };

            }
        }
        [HttpPost]
        [Route("ForgetPassword")]
        public async Task<ResponseAccount> ForgetPassword(string email)
        {
            try
            {
                if (email == null)
                {
                    return new ResponseAccount { Status = "Error", Message = "Enter Your Data!" };

                }
                var response =await _unitOfWork.accountRepository.ForgetPassword(email);
                var callbackUrl = Url.Action(
                      "ResetPassword",
                        "Account",
                        new { token = response.Token, email = email },
                        Request.Scheme
                );

                await _emailSender.SendEmailAsync(
                     email,
                     "Change Your password",
                   callbackUrl);
                return response;
            }
            catch
            {
                return new ResponseAccount { Status = "Error", Message = "Internal Error" };

            }

        }
        [HttpGet]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string token ,string email)
        {
            return Ok(new { Token = token, Email = email });
        }
    

            [HttpPost]
        [Route("ResetPassword")]    
        public async Task<ResponseAccount> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                if (resetPasswordDTO == null)
                {
                    return new ResponseAccount { Status = "Error", Message = "Enter Your Data!" };

                }
                return await _unitOfWork.accountRepository.ResetPassword(resetPasswordDTO);
            }
            catch
            {
                return new ResponseAccount { Status = "Error", Message = "Internal Error" };

            }
        }
    }
}
