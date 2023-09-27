using AutoMapper;
using Data_Access.UnitOfWork.AcountUnitOfWork;
using Data_Access.UnitOfWork.ProductUnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DbModels;
using Models.DTO.Products;
using Models.paging;
using Models.Response;

namespace McShippers_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWorkProduct _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        
        public ProductController(IUnitOfWorkProduct unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;


            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;

        }

        [HttpGet,Route("Product/GetAll")]
        public GenericResponse<List<ProductDto>> GetAll([FromQuery]PagingParameters pagingParameters)
        {
            try
            {
                //    List<Product> products = _unitOfWork.productRepository.GetAll(pagingParameters);
                  List<Product> products = _unitOfWork.productRepository.Get("product2");

                if (products.Count == 0)
                {

                    return new GenericResponse<List<ProductDto>>()
                    {
                        Status = "Sucess",
                        Message = "No Data"

                    };
                }
                else
                {
                    var ProductDto = _mapper.Map<List<ProductDto>>(products);
                    return new GenericResponse<List<ProductDto>>()
                    {
                        Status = "Sucess",
                        Message = "The Process is Sucess",
                        Data = ProductDto

                    };
                }
            }
            catch
            {
                return new GenericResponse<List<ProductDto>>()
                {
                    Status = "Error",
                    Message = "Internal Error"


                };
            }
        }
        [HttpGet, Route("Product/GetOneProduct")]
        public GenericResponse<ProductDto> GetOneProduct(int Id)
        {
            try
            {
                Product products = _unitOfWork.productRepository.Get(Id);
                if (products == null)
                {

                    return new GenericResponse<ProductDto>()
                    {
                        Status = "Sucess",
                        Message = "No Data"

                    };
                }
                else
                {
                    var ProductDto = _mapper.Map<ProductDto>(products);
                    return new GenericResponse<ProductDto>()
                    {
                        Status = "Sucess",
                        Message = "The Process is Sucess",
                        Data = ProductDto

                    };
                }
            }
            catch
            {
                return new GenericResponse<ProductDto>()
                {
                    Status = "Error",
                    Message = "Internal Error"


                };
            }
        }
        [HttpPost, Route("Product/CreateProduct")]
        public GenericResponse<ProductDto> CreateProduct([FromForm]ProductDto productDto)
        {
            try
            {
                if (productDto == null)
                {
                    return new GenericResponse<ProductDto>()
                    {
                        Status = "Error",
                        Message = "Enter Your Data!"

                    };
                }
                else
                {
                    productDto.ProductPhoto = SaveImage(productDto.Photo);

                    var product = _mapper.Map<Product>(productDto);
                    _unitOfWork.productRepository.Add(product);
                    _unitOfWork.Save();
                    return new GenericResponse<ProductDto>()
                    {
                        Status = "Success",
                        Message = "The Process Add is Scucess",
                        Data = productDto

                    };
                }
            }
            catch
            {
                return new GenericResponse<ProductDto>()
                {
                    Status = "Error",
                    Message = "Internal Error"


                };
            }
        }
        [HttpPut, Route("Product/UpdateProduct")]
        public GenericResponse<ProductDto> UpdateProduct(int Id, [FromForm] ProductDto productDto)
        {
            try
            {
                if (productDto == null || Id == null)
                {
                    return new GenericResponse<ProductDto>()
                    {
                        Status = "Error",
                        Message = "Enter Your Data!"

                    };
                }
                else
                {
                    productDto.ProductPhoto = SaveImage(productDto.Photo);

                    var product = _mapper.Map<Product>(productDto);
                    product.Id = Id;
                    _unitOfWork.productRepository.Update(product, Id);
                    _unitOfWork.Save();
                    return new GenericResponse<ProductDto>()
                    {
                        Status = "Success",
                        Message = "The Process Update is Scucess",
                        Data = productDto

                    };
                }
            }
            catch
            {
                return new GenericResponse<ProductDto>()
                {
                    Status = "Error",
                    Message = "Internal Error"


                };
            }
        }
        [HttpDelete, Route("Product/DeleteProduct")]
        public GenericResponse<ProductDto> DeleteProduct(int Id)
        {
            try
            {
                if (Id == 0)
                {
                    return new GenericResponse<ProductDto>()
                    {
                        Status = "Error",
                        Message = "Enter Your Data Correct!"

                    };
                }
                else
                {

                    var product = _unitOfWork.productRepository.Get(Id);
                    if (product == null)
                    {
                        return new GenericResponse<ProductDto>()
                        {
                            Status = "Error",
                            Message = "Not Found This Product!"

                        };
                    }
                    var productDto = _mapper.Map<ProductDto>(product);
                    _unitOfWork.productRepository.Remove(product);
                    _unitOfWork.Save();
                    return new GenericResponse<ProductDto>()
                    {
                        Status = "Success",
                        Message = "The Process Remove Success",
                        Data = productDto

                    };
                }
            }
            catch
            {
                return new GenericResponse<ProductDto>()
                {
                    Status = "Error",
                    Message = "Internal Error"


                };
            }

        }
        [HttpGet, Route("Product/SearchPriduct")]
        public GenericResponse<object> SearchPriduct(string propertyName)
        {
            try
            {
                if (propertyName == null)
                {
                    return new GenericResponse<object>()
                    {
                        Status = "Error",
                        Message = "Enter Your Data!"

                    };
                }
                else
                {
                    var data = _unitOfWork.productRepository.Search(propertyName);
                   if(data == null)
                    {
                        return new GenericResponse<object>()
                        {
                            Status = "Error",
                            Message = "Enter Propert Name Correct",
                        

                        };
                    }else
                    return new GenericResponse<object>()
                    {
                        Status = "Success",
                        Message = "The Process Scearch is Sucess",
                        Data = data

                    };
                }
            }
            catch
            {
                return new GenericResponse<Object>()
                {
                    Status = "Error",
                    Message = "Internal Error"


                };
            }
        }
        private string SaveImage(IFormFile sources)
        {
            Guid obj = Guid.NewGuid();

            var webRootPath = _webHostEnvironment.WebRootPath;
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string path = "";
            string filePath = "";

            path = Path.Combine(webRootPath, "resources");

            string urls = "";
            if (sources != null)
            {
                string name = obj.ToString() + sources.FileName;

                filePath = Path.Combine(path, name);

                //  await studentDto.Certification_Photo.CopyToAsync(filePath);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    sources.CopyToAsync(fileStream);

                }




                urls = obj.ToString() + sources.FileName;
            }
            //var doc = new Document(sources.FileName);
            return urls;
        }

    }
}
