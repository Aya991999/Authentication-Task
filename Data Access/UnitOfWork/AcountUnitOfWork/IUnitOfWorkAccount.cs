using Data_Access.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.UnitOfWork.AcountUnitOfWork
{
    public interface IUnitOfWorkAccount : IDisposable
    {
       
        IAccountRepository accountRepository { get; }
 
    }
}
