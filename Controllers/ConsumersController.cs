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
        private readonly IEnrollmentService enrollmentService;
        public ConsumersController
            (
            IConsumerService consumerService,
            IEnrollmentService enrollmentService
            )
        {
            this.consumerService = consumerService;
            this.enrollmentService = enrollmentService;
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
           // return Ok(await this.consumerService.ImportJob(data));
            return Ok(await this.enrollmentService.ImportJob(data));
        }

        [HttpGet("Download/{brandId}")]
        public async Task<IActionResult> ExportJobError(int brandId)
        {
            var response = await this.consumerService.ExportTextFileConsumerByBrandId(brandId);

            // Create file text index
            if (response.IsSuccess)
            {
                byte[] bytes = System.Convert.FromBase64String(response.Message.Split(',').Last());
                return File(bytes, "text/plain", response.Message.Split(',').First());
            }
            else
            {
                return Ok(response);
            }
        }

        // public async Task<IActionResult> ImportJob(ImportDataBinding data) => Ok(await this.enrollmentService.ImportFile(data));
    }
}