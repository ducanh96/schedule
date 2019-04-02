using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Application.RequestModel
{
    public class BaseRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
