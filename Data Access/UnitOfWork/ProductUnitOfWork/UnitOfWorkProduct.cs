using Data_Access.Data;
using Data_Access.IRepository;
using Data_Access.Repository;
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

namespace Data_Access.UnitOfWork.ProductUnitOfWork
{
    public class UnitOfWorkProduct:IUnitOfWorkProduct
    {
        private readonly DataDbContext _Data;

        public UnitOfWorkProduct(DataDbContext Data)
        {
            _Data = Data;

            productRepository = new ProductRepository(_Data);

        }
        public IProductRepository productRepository { get; set; }

        public IAccountRepository accountRepository { get; set; }

        public void Dispose()
        {
            _Data.Dispose();
        }
        public void Save()
        {
            _Data.SaveChanges();
        }

    }
}
