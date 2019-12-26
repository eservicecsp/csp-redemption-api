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
    public class PromotionTypesController : ControllerBase
    {
        private readonly IPromotionTypeService _promotionTypeService;
        public PromotionTypesController(IPromotionTypeService promotionTypeService)
        {
            _promotionTypeService = promotionTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPromotionTypesAsync()
        {
            return Ok(await _promotionTypeService.GetPromotionTypesAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromotionAsync(int id)
        {
            return Ok(await _promotionTypeService.GetPromotionTypeAsync(id));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(PromotionTypeModel promotionType)
        {
            return Ok(await _promotionTypeService.CreateAsync(promotionType));
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync(PromotionTypeModel promotionType)
        {
            return Ok(await _promotionTypeService.UpdateAsync(promotionType));
        }
    }
}