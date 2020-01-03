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
        [HttpGet("staff/{id}")]
        public async Task<IActionResult> GetStaffsById(int id)
        {
            return Ok(await this.staffService.GetStaffsById(id));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(Staff staff)
        {
            var token = Request.Headers["Authorization"].ToString();
            var brandId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "brandId"));
            var userId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "userId"));
            var requestPassword = Helpers.Argon2Helper.HashPassword(staff.Email, staff.Password);

            var data = new Staff()
            {
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                Email = staff.Email,
                Password = requestPassword,
                Phone = staff.Phone,
                BrandId = brandId,
                CreatedBy = userId,
                CreatedDate = DateTime.Now,
                RoleId = staff.RoleId,
                IsActived = staff.IsActived
        };
            

            return Ok(await this.staffService.CreateAsync(data));
        }
        [HttpPost("update")]
        public async Task<IActionResult> Update(Staff staff)
        {
            var token = Request.Headers["Authorization"].ToString();
            var userId = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "userId"));
            staff.ModifiedBy = userId;
            staff.ModifiedDate = DateTime.Now;
            return Ok(await this.staffService.UpdateAsync(staff));
        }

        [HttpGet("ResetPasswordToken")]
        public async Task<IActionResult> GetResetPasswordToken(string email)
        {
            return Ok(await this.staffService.GetResetPasswordTokenAsync(email));
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(Staff staff)
        {
            return Ok(await this.staffService.ResetPasswordAsync(staff));
        }
    }
}