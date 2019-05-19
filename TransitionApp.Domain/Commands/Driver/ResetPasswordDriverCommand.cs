using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Commands.Driver
{
    public class ResetPasswordDriverCommand : BaseCommand
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public override bool IsValid(object obj = null)
        {
            throw new NotImplementedException();
        }
    }
}
