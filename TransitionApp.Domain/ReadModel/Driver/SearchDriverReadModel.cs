using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.ReadModel.Driver
{
    public class SearchDriverReadModel
    {
        public PagingReadModel PageInfo { get; set; }
        public IEnumerable<DriverReadModel> Drivers { get; set; }
    }
}
