using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.Model.ValueObject;
using TransitionApp.Domain.ReadModel;
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
                var result = await conn.QueryFirstOrDefaultAsync<VehicleReadModel>(sQuery);
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
                               AND (@LicensePlate IS NULL OR LOWER(LicensePlate) like LOWER(@LicensePlate))
                               AND (@VehicleType = 0  OR VehicleType = @VehicleType)
                               AND (@Name IS NULL OR LOWER(Name) like LOWER(@SearchName));
                             
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
                            SELECT Id, LicensePlate, Capacity, VehicleType, Volume, Code, Name, MaxLoad, Driver, Note FROM dbo.Vehicle
                               WHERE (@Code IS NULL OR LOWER(Code) = LOWER(@Code))
                               AND (@LicensePlate IS NULL OR LOWER(LicensePlate) like LOWER(@LicensePlate))
                               AND (@VehicleType = 0  OR VehicleType = @VehicleType)
                               AND (@Name IS NULL OR LOWER(Name) like LOWER(@SearchName))
                               ORDER BY id OFFSET(@page - 1) * @pageSize ROWS FETCH NEXT @pageSize ROWS ONLY;
                           END
                        ELSE
                          BEGIN
                             SELECT Id, LicensePlate, Capacity, VehicleType, Volume, Code, Name, Driver, Note FROM dbo.Vehicle
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
                {
                    page = page,
                    pageSize = pageSize,
                    Code = vehicleModel.Code,
                    LicensePlate = "%" + vehicleModel.LicensePlate + "%",
                    VehicleType = vehicleModel.VehicleType,
                    Name = vehicleModel.Name,
                    SearchName = "%" + vehicleModel.Name + "%",
                }))
                {
                    var vehicle = multi.Read<VehicleReadModel>();
                    var invoiceItems = multi.ReadFirstOrDefault<PagingReadModel>();

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
                var result = conn.QueryFirstOrDefault<bool>(sQuery, new
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
                                Volume
                                Code,
                                VehicleType,
                                Driver,
                                Name,
                                MaxLoad,
                                Note
                                )" +
                    " Values(@LicensePlate, " +
                    "         @Volume, " +
                    "         @Code, " +
                    "         @VehicleType, " +
                    "         @Driver, " +
                    "         @Name, " +
                    "         @MaxLoad, " +
                    "         @Note " +
                    "        ); " +
                    "SELECT Id from Vehicle where Id = CAST(SCOPE_IDENTITY() as int)";
                var result = conn.QueryFirstOrDefault<VehicleModel>(sQuery, new
                {
                    LicensePlate = vehicle.LicensePlate.Number,
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

        public VehicleModel Edit(Vehicle vehicle)
        {
            using (IDbConnection conn = Connection)
            {
                // thieu volumne
                string sQuery = @"Update Vehicle  
                       SET LicensePlate = @LicensePlate,
                                Code =  @Code,
                                VehicleType = @VehicleType,
                                Driver = @Driver,
                                Name = @Name,
                                MaxLoad = @MaxLoad,
                                Note = @Note,
                                Volume = @Volume
                       Where Id = @Id;
                    SELECT @Id;";
                var result = conn.QueryFirstOrDefault<VehicleModel>(sQuery, new
                {
                    LicensePlate = vehicle.LicensePlate.Number,
                    Code = vehicle.Code.Value,
                    Driver = vehicle.Driver.Id.Value,
                    Name = vehicle.Name.Full,
                    MaxLoad = vehicle.MaxLoad.Value,
                    VehicleType = vehicle.VehicleType.Id.Value,
                    Note = vehicle.Note.Value,
                    Volume = vehicle.Volume.Size,
                    Id = vehicle.Id.Value
                });

                Console.Write(result);
                return result;
            }
        }

        public bool ImportExcel(List<Vehicle> vehicles)
        {
            List<VehicleImportExcelModel> vehiclesImport = new List<VehicleImportExcelModel>();
            vehicles.ForEach(x =>
            {
                vehiclesImport.Add(new VehicleImportExcelModel
                {
                    Code = x.Code.Value,
                    Driver = x.Driver.Id,
                    LicensePlate = x.LicensePlate.Number,
                    MaxLoad = x.MaxLoad.Value,
                    Name = x.Name.Full,
                    Note = x.Note.Value,
                    VehicleType = x.VehicleType.Id,
                    Volume = x.Volume.Size
                });
            });

           
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    var trans = conn.BeginTransaction();
                    // thieu volumne
                    string sQuery = @"Insert Into 
                        Vehicle(LicensePlate, 
                                Volume,
                                Code,
                                VehicleType,
                                Driver,
                                Name,
                                MaxLoad,
                                Note
                                ) " +
                           " Values( @LicensePlate, " +
                           "         @Volume, " +
                           "         @Code, " +
                           "         @VehicleType, " +
                           "         @Driver, " +
                           "         @Name, " +
                           "         @MaxLoad, " +
                           "         @Note " +
                           "        ); ";

                    var result = conn.Execute(sQuery, vehiclesImport, transaction: trans);
                    trans.Commit();
                    Console.Write(result);
                    return result > 0;
                }

          


        }

        public bool checkExist(string code)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = "SELECT CASE " +
                    " WHEN EXISTS (SELECT 1 FROM VEHICLE WHERE Lower(Code) = Lower(@Code)) " +
                    " THEN 1 " +
                    " ELSE 0" +
                    " END ";
                var result = conn.QueryFirstOrDefault<bool>(sQuery, new
                {
                    Code = code
                }
                );
                return result;
            }
        }

        #endregion



        private class VehicleImportExcelModel
        {
            public string Code { get; set; }
            public int VehicleType { get; set; }
            public double MaxLoad { get; set; }
            public string Name { get; set; }
            public int Driver { get; set; }
            public string Note { get; set; }
            public string LicensePlate { get; set; }
            public string Volume { get; set; }
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
