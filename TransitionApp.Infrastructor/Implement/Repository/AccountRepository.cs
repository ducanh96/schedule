using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.ReadModel.Account;

namespace TransitionApp.Infrastructor.Implement.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _config;
        public AccountRepository(IConfiguration config)
        {
            _config = config;
        }
        public AccountReadModel Get(string userName, string password)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"Select Id, DisplayName, Role, Status, UserName 
                                From Account Where Lower(UserName) = Lower(@UserName)
                                        AND Password = @Password ;";

                var result = conn.QueryFirstOrDefault<AccountReadModel>(sQuery, new
                {
                    UserName = userName,
                    Password = password
                });
                return result;
            }
        }

        public bool IsExixtAccount(string userName)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"Select Id, DisplayName, Role, Status, UserName 
                                From Account Where Lower(UserName) = Lower(@UserName);";

                var result = conn.QueryFirstOrDefault<AccountReadModel>(sQuery, new
                {
                    UserName = userName
                });
                Console.WriteLine(result);
                return result != null;
            }
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("ConnectionStringTransition"));
            }
        }
    }
}
