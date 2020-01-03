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
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService; 

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBrandsAsync()
        {
            return Ok(await _brandService.GetBrandsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandAsync(int id)
        {
            return Ok(await _brandService.GetBrandAsync(id));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(BrandRegisterRequestModel request)
        {
            return Ok(await _brandService.Register(request));
        }
        [HttpPost("Update")]
        public async Task<IActionResult> Update(BrandRegisterRequestModel request)
        {
            var token = Request.Headers["Authorization"].ToString();
            request.Staff.CreatedBy = Convert.ToInt32(Helpers.JwtHelper.Decrypt(token.Split(' ')[1], "userId"));
            return Ok(await _brandService.UpdateAsync(request));
        }
    }
}