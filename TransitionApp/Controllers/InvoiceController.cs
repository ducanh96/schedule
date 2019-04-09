using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransitionApp.Application.Interface;
using TransitionApp.Application.RequestModel.Invoice;

namespace TransitionApp.Controllers
{
    [Route("transition/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceAppService _invoiceAppService;

        public InvoiceController(IInvoiceAppService invoiceAppService)
        {
            _invoiceAppService = invoiceAppService;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] FilterInvoiceRequest request)
        {
            var result = await _invoiceAppService.GetAll(request);
            return Ok(result);
        }
    }
}