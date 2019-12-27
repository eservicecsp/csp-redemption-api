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
        private readonly IQrCodeService qrCodeService;
        private readonly IProductTypeService productTypeService;
        public CampaignsController
            (
            ICampaignService campaignService,
            IQrCodeService qrCodeService,
            IProductTypeService productTypeService
            )
        {
            this.campaignService = campaignService;
            this.qrCodeService = qrCodeService;
            this.productTypeService = productTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCampaignsByBrandId()
        {
            var token = Request.Headers["Authorization"].ToString();
            var brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));

            return Ok(await this.campaignService.GetCampaignsByBrandIdAsync(brandId));
        }
        [HttpPost("transaction")]
        public async Task<IActionResult> GetTransactionByCampaignsId(PaginationModel data)
        {
            var token = Request.Headers["Authorization"].ToString();
            var brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));

            return Ok(await this.campaignService.GetTransactionByCampaignsIdAsync(data));
        }
        [HttpPost("qrcode")]
        public async Task<IActionResult> GetqrCodeByCampaignsId(PaginationModel data)
        {
            var token = Request.Headers["Authorization"].ToString();
            var brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));

            return Ok(await this.qrCodeService.GetQrCodeByCampaignIdAsync(data));
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
        [HttpGet("detail/{campaignId}")]
        public async Task<IActionResult> GetCampaignsByCampaignId(int campaignId)
        {
            return Ok(await this.campaignService.GetCampaignsByCampaignIdAsync(campaignId));
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateCampaign(Campaign campaign)
        {
            var token = Request.Headers["Authorization"].ToString();
            return Ok(await this.campaignService.UpdateAsync(campaign));
        }

        [HttpGet("productType/{campaignId}")]
        public async Task<IActionResult> GetProductTypesByBrandId(int campaignId)
        {
            var campaign = await this.campaignService.GetCampaignsByCampaignIdAsync(campaignId);
            return Ok(await this.productTypeService.GetProductTypesByBrandIdAsync(campaign.Campaign.BrandId));
        }

    }
}