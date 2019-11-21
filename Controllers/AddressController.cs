using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSP_Redemption_WebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSP_Redemption_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService addressService;

        public AddressController
            (
                IAddressService addressService
            )
        {
            this.addressService = addressService;
        }

        [HttpGet("zone")]
        public async Task<IActionResult> GetZone()
        {
            return Ok(await this.addressService.GetZone());
        }

        [HttpGet("province/{zoneId}")]
        public async Task<IActionResult> GetProvince(int zoneId)
        {
            return Ok(await this.addressService.GetProvinceByZoneId(zoneId));
        }

        [HttpGet("amphur/{provinceCode}")]
        public async Task<IActionResult> GetAmphur(string provinceCode)
        {
            return Ok(await this.addressService.GetAmphurByProvinceCode(provinceCode));
        }

        [HttpGet("tumbol/{amphurCode}")]
        public async Task<IActionResult> GetTumbol(string amphurCode)
        {
            return Ok(await this.addressService.GetTumbolByAmphurCode(amphurCode));
        }
    }
}