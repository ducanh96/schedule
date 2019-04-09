using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Application.RequestModel.Schedule
{
    public class SearchScheduleRequest
    {
        public int Type { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string Name { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
