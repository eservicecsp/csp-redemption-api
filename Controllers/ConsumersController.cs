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
            var token = Request.Headers["Authorization"].ToString();
            data.BrandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));

            return Ok(await this.consumerService.GetConsumersByBrandIdAsync(data));
        }


        [HttpPost("upload")]
        public async Task<IActionResult> ImportJob(ImportDataBinding data)
        {
            var token = Request.Headers["Authorization"].ToString();
            data.brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));
            data.createBy = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "userId"));
            return Ok(await this.consumerService.ImportJob(data));
        }

        // public async Task<IActionResult> ImportJob(ImportDataBinding data) => Ok(await this.enrollmentService.ImportFile(data));
    }
}