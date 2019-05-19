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
        public async Task<IActionResult> GetAll([FromQuery]SearchDriverRequest request, [FromQuery(Name = "VehicleTypeIDs[]")] List<int> vehicleTypeIDs)
        {
            if(request.Page == 0)
            {
                request.Page = 1;
                request.PageSize = 10;
            }
            var result = await _driverAppService.GetAll(request, vehicleTypeIDs);
            return Ok(result);

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _driverAppService.Get(id);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateDriverRequest request)
        {
            var result = await _driverAppService.Create(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]CreateDriverRequest request)
        {
            var result = await _driverAppService.Update(id, request);
            return Ok(result);
        }

        [HttpPost]
        [Route("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var result = await _driverAppService.ImportExcel(file);
            return Ok(result);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _driverAppService.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody]AccountRequest request)
        {
            var result = await _driverAppService.ResetPassword(request);
            return Ok(result);
        }
    }
}