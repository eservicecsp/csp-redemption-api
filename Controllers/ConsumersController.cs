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
    public class ConsumersController : ControllerBase
    {
        private readonly IConsumerService consumerService;
        public ConsumersController(IConsumerService consumerService)
        {
            this.consumerService = consumerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetConsumers(int brandId)
        {
            return Ok(await this.consumerService.GetConsumersByBrandIdAsync(brandId));
        }
    }
}