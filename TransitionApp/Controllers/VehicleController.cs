using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EPPlus.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using TransitionApp.Application.Interface;
using TransitionApp.Application.RequestModel;
using TransitionApp.Application.RequestModel.Vehicle;
using TransitionApp.Application.ResponseModel;

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

        [HttpPost]
        [Route("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var result = await _vehicleAppService.ImportExcel(file);
            return Ok(result);
        }


        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateVehicleRequests request)
        {
            var result = await _vehicleAppService.Edit(request);
            return Ok(result);
        }


        [HttpGet("{id}/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _vehicleAppService.Get(id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(DeleteVehicleRequest request)
        {
            var result = await _vehicleAppService.Delete(request);
            return Ok(result);
        }
    }
}