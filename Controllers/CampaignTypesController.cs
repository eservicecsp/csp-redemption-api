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
    public class CampaignTypesController : ControllerBase
    {
        private readonly ICampaignTypeService campaignTypeService;
        public CampaignTypesController(ICampaignTypeService campaignTypeService)
        {
            this.campaignTypeService = campaignTypeService;
        }

        public async Task<IActionResult> GetCampaignTypes()
        {
            return Ok(await this.campaignTypeService.GetCampaignTypesAsync());
        }
    }
}