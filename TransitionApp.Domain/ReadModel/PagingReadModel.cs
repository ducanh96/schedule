using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.ReadModel
{
    public class PagingReadModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
}
