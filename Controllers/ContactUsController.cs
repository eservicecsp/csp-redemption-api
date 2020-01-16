using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSP_Redemption_WebApi.Entities.Models;
using CSP_Redemption_WebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSP_Redemption_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private readonly IContactUsService contactUsService;

        public ContactUsController
            (
            IContactUsService contactUsService
            )
        {
            this.contactUsService = contactUsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetContactUsAsync()
        {
            var token = Request.Headers["Authorization"].ToString();
            var brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));

            return Ok(await this.contactUsService.GetContactUsAsync(brandId));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateContactUsAsync(ContactUs contactUs)
        {
            var token = Request.Headers["Authorization"].ToString();
            var brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));
            contactUs.BrandId = brandId;
            return Ok(await this.contactUsService.CreateAsyn(contactUs));
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateContactUsAsync(ContactUs contactUs)
        {
            var token = Request.Headers["Authorization"].ToString();
            var brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));
            contactUs.BrandId = brandId;
            return Ok(await this.contactUsService.UpdateAsyn(contactUs));
        }
    }
}