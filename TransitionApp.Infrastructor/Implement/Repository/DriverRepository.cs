using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.ReadModel.Driver;

namespace TransitionApp.Infrastructor.Implement.Repository
{
    public class DriverRepository : IDriverRepository
    {
        private readonly IConfiguration _config;
        public DriverRepository(IConfiguration config)
        {
            _config = config;
        }
        public async Task Add(Driver driver)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"Insert Into Vehicle(LicensePlate, Capacity, Image, TypeVehicle, Volume)" +
                    " Values(@FullName, @NumberLicense, @ExpirationLicense, @Volume) ";
                var result = await conn.ExecuteAsync(sQuery, new
                {
                  

                });

            }
        }

        public async Task<IEnumerable<DriverReadModel>> GetAll()
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"Select Id, Code, Name, PhoneNumber, Status " +
                    " From Driver";
                var result = await conn.QueryAsync<DriverReadModel>(sQuery);
                return result;
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
