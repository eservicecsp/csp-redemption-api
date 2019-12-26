using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSP_Redemption_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeService productTypeService;

        public ProductTypeController
            (
                IProductTypeService productTypeService
            )
        {
            this.productTypeService = productTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductTypesByBrandId()
        {
            var token = Request.Headers["Authorization"].ToString();
            var brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));

            return Ok(await this.productTypeService.GetProductTypesByBrandIdAsync(brandId));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductTypesById(int id)
        {
            return Ok(await this.productTypeService.GetProductTypesByIdAsync(id));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(ProductType productType)
        {
            var token = Request.Headers["Authorization"].ToString();
            var brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));
            var userId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "userId"));

            var data = new ProductType()
            {
                Name = productType.Name,
                Description = productType.Description,
                BrandId = brandId,
                CreatedBy = userId,
                CreatedDate = DateTime.Now,
                IsActived = productType.IsActived
            };


            return Ok(await this.productTypeService.CreateAsync(data));
        }
        [HttpPost("update")]
        public async Task<IActionResult> Update(ProductType productType)
        {
            var token = Request.Headers["Authorization"].ToString();
            var userId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "userId"));
            productType.ModifiedBy = userId;
            productType.ModifiedDate = DateTime.Now;
            return Ok(await this.productTypeService.UpdateAsync(productType));
        }
    }
}