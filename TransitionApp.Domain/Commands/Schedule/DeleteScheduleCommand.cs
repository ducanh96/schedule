using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Commands.Schedule
{
    public class DeleteScheduleCommand : ScheduleCommand
    {
        public int ID { get; set; }
        public override bool IsValid(object obj = null)
        {
            throw new NotImplementedException();
        }
    }
}
