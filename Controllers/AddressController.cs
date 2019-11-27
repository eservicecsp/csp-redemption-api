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

        [HttpGet("zones")]
        public async Task<IActionResult> GetZones()
        {
            return Ok(await this.addressService.GetZones());
        }

        [HttpGet("provinces/{zoneId}")]
        public async Task<IActionResult> GetProvinces(int zoneId)
        {
            return Ok(await this.addressService.GetProvincesByZoneId(zoneId));
        }

        [HttpGet("amphurs/{provinceCode}")]
        public async Task<IActionResult> GetAmphurs(string provinceCode)
        {
            return Ok(await this.addressService.GetAmphursByProvinceCode(provinceCode));
        }

        [HttpGet("tumbols/{amphurCode}")]
        public async Task<IActionResult> GetTumbols(string amphurCode)
        {
            return Ok(await this.addressService.GetTumbolsByAmphurCode(amphurCode));
        }

        [HttpGet("provinces")]
        public async Task<IActionResult> GetProvinces()
        {
            return Ok(await this.addressService.GetProvincesAsync());
        }
    }
}