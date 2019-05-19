using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.Model.ValueObject;

namespace TransitionApp.Domain.Model.Entity
{
    public class Account
    {
        public Account(Password password, UserName userName)
        {
            Password = password;
            UserName = userName;
        }

        public Account(Password password, UserName userName, Role role)
        {
            Password = password;
            UserName = userName;
            Role = role;
        }



        public Password Password { get; set; }
        public UserName UserName { get; set; }
        public Role Role { get; set; }
    }
}
