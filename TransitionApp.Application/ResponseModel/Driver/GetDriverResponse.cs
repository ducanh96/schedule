using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Application.ResponseModel.Driver
{
    public class GetDriverResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public Object Data { get; set; }

    }
}
