using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IProductService
    {
        Task<ProductsResponseModel> GetProductsByBrandIdAsync(int brandId);
        Task<ResponseModel> CreateAsync(Product product);
    }
    public class ProductService: IProductService 
    {
        private readonly ProductRepository productRepository;
        public ProductService(ProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<ProductsResponseModel> GetProductsByBrandIdAsync(int brandId)
        {
            var response = new ProductsResponseModel();

            try
            {
                var products = await this.productRepository.GetProductsByBrandIdAsync(brandId);
                response.Products = new List<ProductModel>();

                foreach (var product in products)
                {
                    response.Products.Add(new ProductModel()
                    {
                        Description = product.Description,
                        CreatedDate = product.CreatedDate,  
                        BrandId = product.BrandId,  
                        CreatedBy = product.CreatedBy,
                        Id = product.Id,
                        Name = product.Name,
                    });
                }

                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ResponseModel> CreateAsync(Product product)
        {
            var response = new ResponseModel();

            try
            {
                response.IsSuccess = await this.productRepository.CreateAsync(product);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
