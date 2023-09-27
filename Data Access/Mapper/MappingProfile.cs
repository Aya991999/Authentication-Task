using AutoMapper;
using Models.DbModels;
using Models.DTO.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Data_Access.Mapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile() { 
        CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
