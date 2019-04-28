using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransitionApp.Application.Interface;
using TransitionApp.Application.RequestModel.Schedule;

namespace TransitionApp.Controllers
{
    [Route("transition/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleAppService _scheduleAppService;
        public ScheduleController(IScheduleAppService scheduleAppService)
        {
            _scheduleAppService = scheduleAppService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateScheduleRequest request)
        {
            var result = await _scheduleAppService.Create(request);
            return Ok(true);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SearchScheduleRequest request)
        {
            var result = await _scheduleAppService.GetAll(request);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _scheduleAppService.Get(id);
            return Ok(result);
        }

    }
}