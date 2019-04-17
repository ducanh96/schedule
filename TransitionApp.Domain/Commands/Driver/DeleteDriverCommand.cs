using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Commands.Driver
{
    public class DeleteDriverCommand : DriverCommand
    {
        public int ID { get; set; }
        public override bool IsValid(object obj = null)
        {
            throw new NotImplementedException();
        }
    }
}
