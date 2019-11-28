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
    public class DealersController : ControllerBase
    {
        private readonly IDealerService dealerService;
        public DealersController(IDealerService dealerService)
        {
            this.dealerService = dealerService;
        }

        public async Task<IActionResult> GetDealersByBrandId()
        {
            var token = Request.Headers["Authorization"].ToString();
            var brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));

            return Ok(await this.dealerService.GetDealersByBrandIdAsync(brandId));
        }
        
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Dealer dealer)
        {
            var token = Request.Headers["Authorization"].ToString();
            var brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));
            var userId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "userId"));

            dealer.BrandId = brandId;
            dealer.CreatedBy = userId;
            dealer.CreatedDate = DateTime.Now;

            return Ok(await this.dealerService.CreateAsync(dealer));
        }
    }
}