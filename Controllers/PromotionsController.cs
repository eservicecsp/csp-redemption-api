using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSP_Redemption_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        private readonly IPromotionSubTypeService _promotionSubTypeService;
        public PromotionsController(
            IPromotionService promotionService,
            IPromotionSubTypeService promotionSubTypeService
        )
        {
            _promotionService = promotionService;
            _promotionSubTypeService = promotionSubTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPromotionsAsync()
        {
            var token = Request.Headers["Authorization"].ToString();
            var brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));

            return Ok(await _promotionService.GetPromotionsAsync(brandId));
        }
        [HttpGet("promotionvalid")]
        public async Task<IActionResult> GetPromotionsValidAsync()
        {
            var token = Request.Headers["Authorization"].ToString();
            var brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));

            return Ok(await _promotionService.GetPromotionsValidAsync(brandId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromotionAsync(int id)
        {
            var token = Request.Headers["Authorization"].ToString();
            var brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));

            return Ok(await _promotionService.GetPromotionAsync(brandId, id));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(PromotionModel promotion)
        {
            var token = Request.Headers["Authorization"].ToString();
            promotion.BrandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));
            promotion.CreatedBy = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "userId"));

            return Ok(await _promotionService.CreateAsync(promotion));
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync(PromotionModel promotion)
        {
            var token = Request.Headers["Authorization"].ToString();
            promotion.BrandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));
            promotion.ModifiedBy = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "userId"));

            return Ok(await _promotionService.UpdateAsync(promotion));
        }

        [HttpGet("promotionsubtypes")]
        public async Task<IActionResult> GetPromotionSubTypesAsync()
        {
            return Ok(await _promotionSubTypeService.GetPromotionSubTypeAsync());
        }
    }
}