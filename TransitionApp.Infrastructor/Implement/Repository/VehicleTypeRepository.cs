using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.ReadModel.VehicleType;

namespace TransitionApp.Infrastructor.Implement.Repository
{
    public class VehicleTypeRepository : IVehicleTypeRepository
    {
        private readonly IConfiguration _config;
        public VehicleTypeRepository(IConfiguration config)
        {
            _config = config;
        }

        #region Write


        public async Task<bool> Add(VehicleType vehicleType)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"Insert Into VehicleType(Code, Name)" +
                    " Values(@Code, @Name) ";
                var result = await conn.ExecuteAsync(sQuery, new
                {
                    Code = vehicleType.Code.Value,
                    Name = vehicleType.Name.Full
                });

                return await Task.FromResult(result > 0);
            }
        }

        public Task<bool> Delete(int vehicleTypeId)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"Delete From VehicleType" +
                    " Where Id = @Id";
                var result = conn.Execute(sQuery, new
                {
                    Id = vehicleTypeId

                });
                return Task.FromResult(result > 0);
            }
        }

        public IEnumerable<VehicleTypeReadModel> GetAll()
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = "SELECT Id, Code, Name " +
                    " FROM VehicleType ";
                var result = conn.Query<VehicleTypeReadModel>(sQuery);
                return result;
            }
        }

        public Task<VehicleTypeReadModel> Get(int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = "SELECT Id, Code, Name " +
                    " FROM VehicleType " +
                    " Where Id = @Id ";

                var result = conn.QueryFirstOrDefault<VehicleTypeReadModel>(sQuery, new
                {
                    Id = id
                });
                return Task.FromResult(result);
            }
        }

        public IEnumerable<VehicleTypeReadModel> GetByIds(List<int> ids)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = "SELECT Id, Code, Name " +
                    " FROM VehicleType " +
                    " WHERE Id IN @Ids;";
                var result = conn.Query<VehicleTypeReadModel>(sQuery, new
                {
                    Ids = ids
                });
                return result;
            }
        }

        #endregion

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("ConnectionStringTransition"));
            }
        }
    }
}
