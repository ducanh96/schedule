using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Application.RequestModel.Driver
{
    public class SearchDriverRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Code { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }

    }
}
