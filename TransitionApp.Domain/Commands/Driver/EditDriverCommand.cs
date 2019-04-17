using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Commands.Driver
{
    public class EditDriverCommand:CreateDriverCommand
    {
        public int Id { get; set; }
    }
}
