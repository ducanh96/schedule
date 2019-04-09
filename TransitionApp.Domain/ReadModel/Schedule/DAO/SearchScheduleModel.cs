using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.ReadModel.Schedule.DAO
{
    public class SearchScheduleModel
    {
        public string Name { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public int Type { get; set; }
    }
}
