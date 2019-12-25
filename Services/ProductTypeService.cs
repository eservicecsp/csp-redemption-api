using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IProductTypeService
    {
        Task<ResponseModel> CreateAsync(ProductType productType);
        Task<ResponseModel> UpdateAsync(ProductType productType);
        Task<ProductsTypeResponseModel> GetProductTypesByBrandIdAsync(int brandId);
    }
    public class ProductTypeService : IProductTypeService
    {
        private readonly IProductTypeRepository productTypeRepository;

        public ProductTypeService
            (
                IProductTypeRepository productTypeRepository
            )
        {
            this.productTypeRepository = productTypeRepository;
        }
        public async Task<ResponseModel> CreateAsync(ProductType productType)
        {
            var response = new ResponseModel();
            try
            {
                response.IsSuccess = await this.productTypeRepository.CreateAsync(productType);
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> UpdateAsync(ProductType productType)
        {
            var response = new ResponseModel();
            try
            {
                response.IsSuccess = await this.productTypeRepository.UpdateAsync(productType);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ProductsTypeResponseModel> GetProductTypesByBrandIdAsync(int brandId)
        {
            var response = new ProductsTypeResponseModel();
            try
            {
                var productTypesDb = await this.productTypeRepository.GetProductTypesByBrandIdAsync(brandId);
                var productTypes = new List<ProductTypeModel>();
                if(productTypesDb != null)
                {
                   foreach(var item in productTypesDb)
                    {
                        productTypes.Add(new ProductTypeModel() { 
                            Id = item.Id,
                            Name = item.Name,
                            Description = item.Description,
                            BrandId = item.BrandId,
                            IsActived = item.IsActived,
                        });
                    }
                }
                response.ProductTypes = productTypes;
                response.IsSuccess = true;
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
