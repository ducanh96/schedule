using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.ReadModel.Schedule
{
    public class SearchScheduleReadModel
    {
        public IEnumerable<ScheduleReadModel> Schedules { get; set; }
        public PagingReadModel PageInfo { get; set; }

    }
}
