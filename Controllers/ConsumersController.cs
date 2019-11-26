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
    public class ConsumersController : ControllerBase
    {
        private readonly IConsumerService consumerService;
        public ConsumersController(IConsumerService consumerService)
        {
            this.consumerService = consumerService;
        }

        [HttpPost]
        public async Task<IActionResult> GetConsumers(PaginationModel data)
        {
            return Ok(await this.consumerService.GetConsumersByBrandIdAsync(data));
        }
    }
}