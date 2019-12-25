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
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService promotionService;
        public PromotionsController(IPromotionService promotionService)
        {
            this.promotionService = promotionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPromotionsAsync()
        {
            var token = Request.Headers["Authorization"].ToString();
            var brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));

            return Ok(await this.promotionService.GetPromotionsAsync(brandId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromotionByIdAsync(int id)
        {
            var token = Request.Headers["Authorization"].ToString();
            var brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));

            return Ok(await this.promotionService.GetPromotionsByIdAsync(brandId, id));
        }
    }
}