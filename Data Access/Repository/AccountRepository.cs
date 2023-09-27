using Data_Access.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.DbModels;
using Models.DTO.Account;
using Models.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Azure.Core;
using System.Security.Policy;
using System.Web;
using Models.SendEmail;
using Models.Response;

namespace Data_Access.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public readonly IConfiguration _configuration;
        private readonly JWT _jwt;
        
        public AccountRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IOptions<JWT> jwt)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _jwt = jwt.Value;
         
        }
        public async Task<ResponseAccount> Login( LoginDTO loginDTO)
        {
            var user = await userManager.FindByNameAsync(loginDTO.Username);

            if (user == null || !await userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                return new ResponseAccount { Status = "Error", Message = "UserName Or Password Is Not Correct" };

            }

            var jwtToken = await CreateJwtToken(user);
            return new ResponseAccount { Status = "Success", Message = "User Logined successfully!", Token = new JwtSecurityTokenHandler().WriteToken(jwtToken) };



        }

        public async Task<ResponseAccount> Registration(RegisterDTO registerDTO)
        {
            var userNameExists = await userManager.FindByNameAsync(registerDTO.Username);
            if (userNameExists != null)
                return new ResponseAccount { Status = "Error", Message = "UserName already exists Before!" };
            var EmailExists = await userManager.FindByNameAsync(registerDTO.Email);
            if (EmailExists != null)
                return new ResponseAccount { Status = "Error", Message = "Email already exists Before!" };

            if (registerDTO.Password != registerDTO.ConfirmPassword)
                return new ResponseAccount { Status = "Error", Message = "Password and Confirm Password Must be Same!" };
            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerDTO.Username
            };
            
            var result = await userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
                return new ResponseAccount { Status = "Error", Message = "User creation failed! Please check user details and try again." };

            var jwtToken = await CreateJwtToken(user);
            return new ResponseAccount { Status = "Success", Message = "User created successfully!", Token = new JwtSecurityTokenHandler().WriteToken(jwtToken) };


        }
        public async Task<ResponseAccount> ForgetPassword(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new ResponseAccount { Status = "Error", Message = "Email Not Found" };

            }

            var Token = await userManager.GeneratePasswordResetTokenAsync(user);
            Token = HttpUtility.UrlEncode(Token);
            return new ResponseAccount { Status = "Success", Message = "Email Send Please go to Gamil to change Password" ,Token= Token };


        }
        public async Task<ResponseAccount> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordDTO.Email);

            if (user == null)
            {
                return new ResponseAccount { Status = "Error", Message = "Please Enter Your Email!" };

            }
            var result = await userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(resetPasswordDTO.Token), resetPasswordDTO.Password);
            if (!result.Succeeded)
                return new ResponseAccount { Status = "Error", Message = "Enter Password Correct!" };

            return new ResponseAccount { Status = "Sucess", Message = "Password Changed Success" };

        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.ValidIssuer,
                audience: _jwt.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
