using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransitionApp.Application.Interface;
using TransitionApp.Application.RequestModel.VehicleType;

namespace TransitionApp.Controllers
{
    [Route("transition/[controller]")]
    [ApiController]
    public class VehicleTypeController : ControllerBase
    {
        private readonly IVehicleTypeAppService _vehicleTypeAppService;

        public VehicleTypeController(IVehicleTypeAppService vehicleTypeAppService)
        {
            _vehicleTypeAppService = vehicleTypeAppService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _vehicleTypeAppService.GetAll();
            return Ok(result);

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateVehicleTypeRequest request)
        {
           var result = await _vehicleTypeAppService.Create(request);
            return Ok(result);
        }

        // DELETE vehicles-type/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _vehicleTypeAppService.Delete(id);
            return Ok(result);
        }
    }
}