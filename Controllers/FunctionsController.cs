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
    public class FunctionsController : ControllerBase
    {
        private readonly IFunctionService functionService;
        public FunctionsController(IFunctionService functionService)
        {
            this.functionService = functionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFunction(string type)
        {
            return Ok(await this.functionService.GetFunctionsAsync(type.ToLower()));
        }
    }
}