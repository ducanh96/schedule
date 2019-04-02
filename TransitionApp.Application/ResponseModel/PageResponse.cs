using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Application.ResponseModel
{
    public class PageResponse
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
}
