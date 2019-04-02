using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Application.ResponseModel.Driver
{
    public class DriverResponse
    {
        public string Message { get; set; }
        public PageResponse Metadata { get; set; }
        public bool Success { get; set; }
        public IEnumerable<Object> Data { get; set; }
      
    }
}
