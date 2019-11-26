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
    public class StaffsController : ControllerBase
    {
        private readonly IStaffService staffService;

        public StaffsController(IStaffService staffService)
        {
            this.staffService = staffService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Authenticate(Staff staff)
        {
            var response = await this.staffService.Authenticate(staff);
            if (!response.IsSuccess)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpGet("Authorize")]
        public async Task<IActionResult> Authorize()
        {
            var response = await this.staffService.Authorize(Request.Headers["Authorization"]);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetStaffsByBrandId()
        {
            var token = Request.Headers["Authorization"].ToString();
            var brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));

            return Ok(await this.staffService.GetStaffsByBrandIdAsync(brandId));
        }
    }
}