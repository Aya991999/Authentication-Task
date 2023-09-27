using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.paging
{
    public class PageList<T>:List<T>
    {
        public int Curentpage { get; set; }
        public int PageSize { get; set; }
       

        public PageList(List<T> items, int pageNum, int pageSize)
        {

            PageSize = pageSize;
            Curentpage = pageNum;
            AddRange(items);



        }
        public static PageList<T> GetPageList(IQueryable<T> sourcs, int pageNum, int pageSize)
        {

            var items = sourcs.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
            return new PageList<T>(items, pageNum, pageSize);
        }
        public static bool PageListNext(IQueryable<T> sourcs, int pageNum, int pageSize)
        {

            var items = sourcs.Skip((pageNum - 1) * pageSize).Take(pageSize + 1).ToList();
            if (items.Count < pageSize + 1) return false;
            else
                return true;

        }
    
    }
}
