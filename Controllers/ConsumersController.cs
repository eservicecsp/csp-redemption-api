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

        [HttpGet("Download")]
        public async Task<IActionResult> ExportJobError(
            int startAge, int endAge,
            int birthOfMonth = 0,
            string phone = null,
            string email = null,
            bool isSkincare = false,
            bool isMakeup = false,
            bool isBodycare = false,
            bool isSupplement = false,
            string productTypes = ""
            )
        {

            var token = Request.Headers["Authorization"].ToString();
            int brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));
            List<int> xxx ;
            var data = new FiltersModel()
            {
                startAge = startAge,
                endAge = endAge,
                birthOfMonth = birthOfMonth,
                phone = phone,
                email = email,
                isSkincare = isSkincare,
                isMakeup = isMakeup,
                isBodycare = isBodycare,
                isSupplements = isSupplement ,

            };
            if (productTypes!= null && productTypes != "undefined")
            {
                data.productTypes = productTypes.Split(',').Select(Int32.Parse).ToList();
            }

           
            var response = await this.consumerService.ExportTextFileConsumerByBrandId(data, brandId);

            // Create file text index
            if (response.IsSuccess)
            {
                byte[] bytes = System.Convert.FromBase64String(response.Message.Split(',').Last());
                //byte[] bytes = response.File;
                return File(bytes, "text/plain", response.Message.Split(',').First());
            }
            else
            {
                return Ok(response);
            }
        }

        // public async Task<IActionResult> ImportJob(ImportDataBinding data) => Ok(await this.enrollmentService.ImportFile(data));
        [HttpPost("SendSelected")]
        public async Task<IActionResult> SendSelected(List<Consumer> consumers, string channel, int promotion)
        {
            var token = Request.Headers["Authorization"].ToString();
            return Ok(await this.consumerService.SendSelected(consumers, channel, Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId")), promotion));
        }

        [HttpPost("SendAll")]
        public async Task<IActionResult> SendAll(FiltersModel data, string channel, int promotion)
        {
            var dataModel = new PaginationModel()
            {
                filters = data
            };
            var token = Request.Headers["Authorization"].ToString();
            return Ok(await this.consumerService.SendAll(dataModel, channel, Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId")), promotion));
        }

        [HttpGet("Member")]
        public async Task<IActionResult> GetMember(string phone, int brandId)
        {
            return Ok(await this.consumerService.GetConsumerByPhoneAndBrandIdAsync(phone, brandId));
        }
    }
}