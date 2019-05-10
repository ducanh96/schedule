using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.ReadModel.Account;
using TransitionApp.Service.Interface;

namespace TransitionApp.Service.Implement
{
    public class AccountService:IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository  accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public AccountReadModel Get(string userName, string password)
        {
            return _accountRepository.Get(userName, password);
        }

        public bool IsExistAccount(string userName)
        {
            return _accountRepository.IsExixtAccount(userName);
        }
    }
}
