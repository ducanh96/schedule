using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Application.ResponseModel.Driver
{
    public class CreateDriverResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public int Data { get; set; }
    }
}
