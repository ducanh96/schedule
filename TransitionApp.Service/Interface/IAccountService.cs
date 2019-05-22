using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.ReadModel.Account;

namespace TransitionApp.Service.Interface
{
    public interface IAccountService
    {
        AccountReadModel Get(string userName, string password);
        bool IsExistAccount(string userName);
        bool UpdateAccount(string userName, string password);
       
    }
}
