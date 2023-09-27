using Models.DbModels;
using Models.paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.IRepository
{
    public interface IProductRepository
    {


        PageList<Product> GetAll(PagingParameters pagingParameters);


        void Add(Product entity);
       
        void Update(Product entity, long id);
     

        void Remove(Product entity);
        void Remove(long Id);
        Product Get(long Id);
        object Search(string property);
        List<Product> Get(string name);
    }
}
