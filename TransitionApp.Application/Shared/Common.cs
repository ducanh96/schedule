using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TransitionApp.Application.Shared
{
    public class Common
    {
        public enum ResponseCode : int
        {
            [Display(Name = "Success")]
            OK = 1,
            [Display(Name = "Fail")]
            Fail = 2
        }
        
    }
}
