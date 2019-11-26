using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Models;
using CSP_Redemption_WebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSP_Redemption_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignsController : ControllerBase
    {
        private readonly ICampaignService campaignService;
        public CampaignsController(ICampaignService campaignService)
        {
            this.campaignService = campaignService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCampaignsByBrandId()
        {
            var token = Request.Headers["Authorization"].ToString();
            var brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));

            return Ok(await this.campaignService.GetCampaignsByBrandIdAsync(brandId));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCampaign(CreateCampaignRequestModel requestModel)
        {
            var token = Request.Headers["Authorization"].ToString();
            requestModel.Campaign.BrandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));
            requestModel.Campaign.CreatedBy = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "userId"));
            requestModel.Campaign.CreatedDate = DateTime.Now;

            return Ok(await this.campaignService.CreateCampaignAsync(requestModel));
        } 
    }
}