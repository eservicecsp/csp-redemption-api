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
    public class RedemptionController : ControllerBase
    {
        private readonly IConsumerService consumerService;

        public RedemptionController
            (
                 IConsumerService consumerService
            )
        {
            this.consumerService = consumerService;
        }

        //[HttpPost]
        //public async Task<IActionResult> Redemption(CheckExistConsumerRequestModel consumerRequest)
        //{
        //    return Ok(await this.consumerService.Redemption(consumerRequest));
        //}


        
        [HttpPost]
        public async Task<IActionResult> Register(ConsumerRequestModel consumerRequest)
        {
            return Ok(await this.consumerService.Register(consumerRequest));
        }


        [HttpPost("IsExist")]
        public async Task<IActionResult> IsExist(CheckExistConsumerRequestModel dataConsumer)
        {
            return Ok(await this.consumerService.IsExist(dataConsumer));
        }
        [HttpPost("enrollment")]
        public async Task<IActionResult> enrollment(CheckExistConsumerRequestModel dataConsumer)
        {
            return Ok(await this.consumerService.RegisterEnrollment(dataConsumer));
        }
        [HttpPost("consumer")]
        public async Task<IActionResult> registerConsumerEnrollment(ConsumerRequestModel dataConsumer)
        {
            return Ok(await this.consumerService.registerConsumerEnrollment(dataConsumer));
        }
    }
}