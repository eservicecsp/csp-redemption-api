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
    public class ChartsController : ControllerBase
    {
        private readonly IChartService chartService;

        public ChartsController
            (
            IChartService chartService
            )
        {
            this.chartService = chartService;
        }

        [HttpGet("transaction/{campaignId}")]
        public async Task<IActionResult> GetChartTransaction(int campaignId)
        {
            return Ok(await this.chartService.GetChartTransaction(campaignId));
        }
        [HttpGet("qrCode/{campaignId}")]
        public async Task<IActionResult> GetChartQrCode(int campaignId)
        {
            return Ok(await this.chartService.GetChartQrCode(campaignId));
        }
    }
}