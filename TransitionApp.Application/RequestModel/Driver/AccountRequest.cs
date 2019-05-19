using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Application.RequestModel.Driver
{
    public class AccountRequest
    {
        public string DriverID { get; set; }
        public string NewPassword { get; set; }
    }
}
