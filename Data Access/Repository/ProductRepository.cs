using Data_Access.Data;
using Data_Access.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.DbModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Data.SqlClient;

namespace Data_Access.Repository
{
    public class ProductRepository : IProductRepository 
    {
        private readonly DataDbContext _db;
       
       
        private readonly IHttpContextAccessor _httpContextAccessor;

        
        public ProductRepository(DataDbContext DB)
        {
            _db = DB;
          
           
            
            


        }
        
        public PageList<Product> GetAll(PagingParameters pagingParameters)
        {
           // return _db.Products;
            return PageList<Product>.GetPageList(FindAll(), pagingParameters._PageIndex, pagingParameters._PageSize);

        }
        public void Add(Product entity)
        {

            _db.Add(entity);
        }
        public Product Get(long Id)
        {



            Product query = _db.Products.FirstOrDefault(x => x.Id==Id);
            return query;
        }
        public List<Product> Get(string name)
        {

            var param = new SqlParameter("@ProductName", name);

            var query = _db.Products.FromSqlRaw("dbo.GetProduct2 @ProductName", param).ToList();
            return query;
        }
        public void Update(Product entity, long id)
        {
            Product currentEntity = Get(id);

            
            _db.Entry<Product>(currentEntity).CurrentValues.SetValues(entity);


        }
       public object Search(string property)
        {
            var prop=new Product().GetType().GetProperty(property);
            if (prop == null)
                return null;
            var parameter = Expression.Parameter(typeof(Product));
            var propertyEx = Expression.Property(parameter, property);
            var propAsObject = Expression.Convert(propertyEx, typeof(object));

            var experation= Expression.Lambda<Func<Product, object>>(propAsObject, parameter);
         
            var Data = _db.Products.Select(experation).ToList();
            return Data;
        }





        public void Remove(Product entity)
        {
            _db.Remove(entity);
        }
        public void Remove(long Id)
        {
            Product entity = _db.Products.Find(Id);
            Remove(entity);
        }


        private IQueryable<Product> FindAll()
        {
            return _db.Set<Product>();
        }

       
    }
}
