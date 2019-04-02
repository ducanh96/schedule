using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static TransitionApp.Application.Shared.Common;

namespace TransitionApp.Application.ResponseModel
{
    public class BaseResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public BaseResponse(string message = "", ResponseCode code = ResponseCode.Fail)
        {
            Code = (int)code;
            Message = message;

        }

        
        public BaseResponse() { }
    }
   
}
