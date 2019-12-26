using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}