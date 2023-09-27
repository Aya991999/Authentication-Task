using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Products
{
    public class ProductDto
    {
       
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public IFormFile Photo { get; set; }
        public string ProductPhoto { get; set;}
    }
}
