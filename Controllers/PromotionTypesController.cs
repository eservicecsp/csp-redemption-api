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
    public class PromotionTypesController : ControllerBase
    {
        private readonly IPromotionTypeService promotionTypeService;
        public PromotionTypesController(IPromotionTypeService promotionTypeService)
        {
            this.promotionTypeService = promotionTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPromotionTypesAsync()
        {
            return Ok(await this.promotionTypeService.GetPromotionTypesAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromotionAsync(int id)
        {
            return Ok(await this.promotionTypeService.GetPromotionTypeAsync(id));
        }
    }
}