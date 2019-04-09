using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.ReadModel;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using TransitionApp.Domain.Model.Entity;
using System.Linq;
using TransitionApp.Domain.Model.ValueObject;
using TransitionApp.Domain.ReadModel.Vehicle;

namespace TransitionApp.Infrastructor.Implement.Repository
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly IConfiguration _config;
        public VehicleRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<VehicleReadModel> Get()
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = "SELECT ID, LicensePlate FROM Vehicle";
                conn.Open();
                var result = await conn.QueryFirstAsync<VehicleReadModel>(sQuery);
                conn.Close();
                return result;
            }

        }

        #region Read
        public VehicleReadModel GetById(int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = "SELECT Id, LicensePlate, Capacity, VehicleType, Volume, Code, Name, MaxLoad, Driver, Note " +
                    "FROM Vehicle Where Id = @Id";
                var result = conn.QueryFirst<VehicleReadModel>(sQuery, new
                {
                    Id = id
                });
                return result;
            }
        }

        public SearchVehicleReadModel GetAll(int page, int pageSize, SearchVehicleModel vehicleModel)
        {
            using (IDbConnection conn = Connection)
            {
                //string sQuery = "SELECT Id, LicensePlate, Capacity, VehicleType, Volume, Code " +
                //    " FROM Vehicle ";
                string sQuery = @"BEGIN
                                DECLARE @totalRow INT-- tong ban ghi
                                , @numPageTemp FLOAT  --temp cho viec tinh tong so page
                                , @maxPage INT; --so page thuc te
                            SELECT @totalRow = COUNT(*) FROM dbo.Vehicle
                               WHERE (@Code IS NULL OR LOWER(Code) = LOWER(@Code))
                               AND (@LicensePlate IS NULL OR LOWER(LicensePlate) = LOWER(@LicensePlate))
                               AND (@VehicleType = 0  OR VehicleType = @VehicleType)
                               AND (@Name IS NULL OR Name like '%@Name%');
                             
                            SELECT @numPageTemp = CAST(@totalRow AS FLOAT) / @pageSize;
                        BEGIN
                           IF @numPageTemp > CAST(@numPageTemp AS INT)
                            SELECT @maxPage = CAST(@numPageTemp AS INT) + 1;
                           IF @numPageTemp = CAST(@numPageTemp AS INT)
                            SELECT @maxPage = CAST(@numPageTemp AS INT);
                           IF @page > @maxPage
                            SELECT @page = @maxPage
                         END
                        
                        IF @totalRow > 0
                           BEGIN 
                            SELECT Id, LicensePlate, Capacity, VehicleType, Volume, Code, Name, MaxLoad FROM dbo.Vehicle
                               WHERE (@Code IS NULL OR LOWER(Code) = LOWER(@Code))
                               AND (@LicensePlate IS NULL OR LOWER(LicensePlate) = LOWER(@LicensePlate))
                               AND (@VehicleType = 0  OR VehicleType = @VehicleType)
                               AND (@Name IS NULL OR Name like '%@Name%')
                               ORDER BY id OFFSET(@page - 1) * @pageSize ROWS FETCH NEXT @pageSize ROWS ONLY;
                           END
                        ELSE
                          BEGIN
                             SELECT Id, LicensePlate, Capacity, VehicleType, Volume, Code, Name FROM dbo.Vehicle
                               WHERE 1 != 1;
                          END
                     
                    
                     SELECT @page as Page, @pageSize as PageSize, @totalRow as Total; 
                    END ";
              
                //var result = conn.QueryAsync<VehicleReadModel>(sQuery, new
                //{
                //    page = page,
                //    pageSize = pageSize
                //});
              

                    using (var multi = conn.QueryMultiple(sQuery, new
                        {   page = page,
                            pageSize = pageSize,
                            Code  = vehicleModel.Code,
                            LicensePlate = vehicleModel.LicensePlate,
                            VehicleType = vehicleModel.VehicleType,
                            Name = vehicleModel.Name,
                    }))
                    {
                        var vehicle = multi.Read<VehicleReadModel>();
                        var invoiceItems = multi.ReadFirst<PagingReadModel>();
                      
                    SearchVehicleReadModel searchVehicle = new SearchVehicleReadModel
                    {
                        Vehicles = vehicle,
                        PageInfo = invoiceItems
                    };
                    return searchVehicle;
                }

               
            }
        }

        public bool checkExist(int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = "SELECT CASE " +
                    " WHEN EXISTS (SELECT 1 FROM VEHICLE WHERE ID = @Id) " +
                    " THEN 1 " +
                    " ELSE 0" +
                    " END ";
                var result = conn.QueryFirst<bool>(sQuery, new
                {
                    Id = id
                }
                );
                return result;
            }
        }




        #endregion

        #region Write
        public VehicleModel Add(Vehicle vehicle)
        {
            using (IDbConnection conn = Connection)
            {
                // thieu volumne
                string sQuery = @"Insert Into 
                        Vehicle(LicensePlate, 
                               
                                Code,
                                VehicleType,
                                Driver,
                                Name,
                                MaxLoad,
                                Note
                                )" +
                    " Values(@LicensePlate, " +
                   
                    "         @Code, " +
                    "         @VehicleType, " +
                    "         @Driver, " +
                    "         @Name, " +
                    "         @MaxLoad, " +
                    "         @Note " +
                    "        ); " +
                    "SELECT Id from Vehicle where Id = CAST(SCOPE_IDENTITY() as int)";
                var result = conn.QueryFirst<VehicleModel>(sQuery, new
                {
                    LicensePlate  = vehicle.LicensePlate.Number,
                    Code = vehicle.Code.Value,
                    Driver = vehicle.Driver.Id.Value,
                    Name = vehicle.Name.Full,
                    MaxLoad = vehicle.MaxLoad.Value,
                    VehicleType = vehicle.VehicleType.Id.Value,
                    Note = vehicle.Note.Value
                });

                Console.Write(result);
                return result;
            }
        }


        public Task Delete(Identity identity)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = "Delete FROM Vehicle v" +
                    " Where v.Id = @Id";

                var result = conn.ExecuteAsync(sQuery, new
                {
                    Id = identity.Value
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
