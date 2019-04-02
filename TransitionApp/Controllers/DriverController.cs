using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransitionApp.Application.Interface;
using TransitionApp.Application.RequestModel.Driver;

namespace TransitionApp.Controllers
{
    [Route("transition/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverAppService _driverAppService;
        public DriverController(IDriverAppService driverAppService)
        {
            _driverAppService = driverAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]SearchDriverRequest request)
        {
            var result = await _driverAppService.GetAll(request);
            return Ok(result);

        }
    }
}