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
            this._brandService = brandService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(BrandRegisterRequestModel request)
        {
            return Ok(await this._brandService.Register(request));
        }
    }
}