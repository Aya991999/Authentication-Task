using Data_Access.Data;
using Data_Access.IRepository;
using Data_Access.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Models.DbModels;
using Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.UnitOfWork.AcountUnitOfWork
{
    public class UnitOfWorkAccount : IUnitOfWorkAccount
    {
        private readonly DataDbContext _Data;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly JWT _jwt;
        public UnitOfWorkAccount(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _jwt = jwt.Value;
            accountRepository = new AccountRepository(_userManager, _roleManager, _configuration, jwt);
        }
        
        public IAccountRepository accountRepository { get; set; }

        public void Dispose()
        {
            _userManager.Dispose();
        }
        
    }
}
