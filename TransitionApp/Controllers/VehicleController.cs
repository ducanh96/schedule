using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransitionApp.Application.Interface;
using TransitionApp.Application.RequestModel;
using TransitionApp.Application.RequestModel.Vehicle;

namespace TransitionApp.Controllers
{
    [Route("transition/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleAppService _vehicleAppService;
        
        public VehicleController(IVehicleAppService vehicleAppService)
        {
            _vehicleAppService = vehicleAppService;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] SearchVehicleRequest searchVehicle)
        {
            var result =  await _vehicleAppService.GetAll(searchVehicle);
            return Ok(result);

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateVehicleRequests request)
        {
            var result = await _vehicleAppService.Create(request);
            return Ok(result);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }


        [HttpGet("{id}/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _vehicleAppService.Get(id);
            return Ok(result);
        }
    }
}