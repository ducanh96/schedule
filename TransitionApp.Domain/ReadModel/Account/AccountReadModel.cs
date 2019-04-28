using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.ReadModel.Account
{
    public class AccountReadModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public int Status { get; set; }
        
    }
}
