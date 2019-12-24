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
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService enrollmentService;

        public EnrollmentsController
            (
            IEnrollmentService enrollmentService
            )
        {
            this.enrollmentService = enrollmentService;
        }

        [HttpPost]
        public async Task<IActionResult> GetEnrollments(PaginationModel data)
        {
            var token = Request.Headers["Authorization"].ToString();
            data.BrandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));

            return Ok(await this.enrollmentService.GetEnrollmentsByCampaignIdAsync(data));
        }

        [HttpPost("SendSelected")]
        public async Task<IActionResult> SendSelected(List<Enrollment> enrollments, string channel,int campaignId)
        {
            return Ok(await this.enrollmentService.SendSelected(enrollments, channel, campaignId));
        }

        [HttpPost("SendAll")]
        public async Task<IActionResult> SendAll(PaginationModel data, string channel, int campaignId)
        {
            return Ok(await this.enrollmentService.SendAll(data, channel, campaignId));
        }
    }
}