using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.ReadModel;
using TransitionApp.Domain.ReadModel.Customer;
using TransitionApp.Domain.ReadModel.Driver;
using TransitionApp.Domain.ReadModel.Driver.DAO;

namespace TransitionApp.Infrastructor.Implement.Repository
{
    public class DriverRepository : IDriverRepository
    {
        private readonly IConfiguration _config;
        public DriverRepository(IConfiguration config)
        {
            _config = config;
        }
        public DriverModel Add(Driver driver)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery =
                    @"Insert Into Driver(Name
                                            , Code
                                            , PhoneNumber
                                            , Status
                                            , UserID
                                            , StartDate
                                            , DoB
                                            , IDCardNumber
                                            , Note
                                            , Sex
                                            , VehicleTypeIDs
                                            , City
                                            , Country
                                            , District
                                            , Street
                                            , StreetNumber)
                                Values(@Name
                                    , @Code
                                    , @PhoneNumber
                                    , @Status
                                    , @UserID
                                    , @StartDate
                                    , @DoB
                                    , @IDCardNumber
                                    , @Note
                                    , @Sex
                                    , @VehicleTypeIDs
                                    , @City
                                    , @Country
                                    , @District
                                    , @Street
                                    , @StreetNumber); 
                           SELECT Id from Driver where Id = CAST(SCOPE_IDENTITY() as int)";

                var result = conn.QueryFirstOrDefault<DriverModel>(sQuery, new
                {
                    Name = driver.Name.Full,
                    Code = driver.Code.Value,
                    PhoneNumber = driver.PhoneNumber.Value,
                    Status = driver.Status.Value,
                    UserID = driver.UserID.Value,
                    StartDate = driver.StartDate.Value,
                    DoB = driver.DoB.Value,
                    IDCardNumber = driver.IDCardNumber.Value,
                    Note = driver.Note.Value,
                    Sex = driver.Sex.Value,
                    VehicleTypeIDs = driver.VehicleTypeIDs.Value,
                    City = driver.Address.City,
                    Country = driver.Address.Country,
                    District = driver.Address.District,
                    Street = driver.Address.Street,
                    StreetNumber = driver.Address.StreetNumber
                });
                return result;
            }
        }



        public SearchDriverReadModel GetAll(int page, int pageSize, SearchDriverModel driverModel)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"BEGIN
                        DECLARE @totalRow INT-- tong ban ghi
                        SELECT @totalRow = Count(*) FROM dbo.Driver
                        WHERE (@Code IS NULL OR LOWER(Code) = LOWER(@Code))
                            AND (@PhoneNumber IS NULL OR PhoneNumber LIKE @SearchPhoneNumber )
                            AND (@Name IS NULL OR LOWER(Name) like LOWER(@SearchName))
                            AND (@VehicleTypeIDs IS NULL OR VehicleTypeIDs like @SearchVehicleTypeIDs) ;

                     Select
                            [Id], [Code], [Name], [PhoneNumber], [Status], [UserID],
                            [City], [Country], [District], [Street], [StreetNumber],
                            [DoB], [IDCardNumber], [Note], [Sex], [StartDate], [VehicleTypeIDs]

                        From Driver
                        WHERE (@Code IS NULL OR LOWER(Code) = LOWER(@Code))
                        AND (@PhoneNumber IS NULL OR PhoneNumber LIKE @SearchPhoneNumber )
                        AND (@Name IS NULL OR LOWER(Name) like LOWER(@SearchName))
                        AND (@VehicleTypeIDs IS NULL OR VehicleTypeIDs like @SearchVehicleTypeIDs)
                    ORDER BY Code OFFSET(@page - 1) * @pageSize ROWS FETCH NEXT @pageSize ROWS ONLY;
                    SELECT @page as Page, @pageSize as PageSize, @totalRow as Total;
                    END ";

                using (var multi = conn.QueryMultiple(sQuery, new
                {
                    page = page,
                    pageSize = pageSize,
                    Code = driverModel.Code,
                    Name = driverModel.Name,
                    PhoneNumber = driverModel.PhoneNumber,
                    SearchPhoneNumber = driverModel.PhoneNumber + "%",
                    SearchName = "%" + driverModel.Name + "%",
                    VehicleTypeIDs = driverModel.VehicleTypeIDs,
                    SearchVehicleTypeIDs = "%" + driverModel.VehicleTypeIDs + "%"
                }))
                {
                    var drivers = multi.Read<DriverReadModel>();
                    var pageData = multi.ReadFirst<PagingReadModel>();

                    SearchDriverReadModel searchVehicle = new SearchDriverReadModel
                    {
                        Drivers = drivers,
                        PageInfo = pageData
                    };
                    return searchVehicle;
                }

            }
        }

        public DriverReadModel Get(int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"Select  [Id], [Code], [Name], [PhoneNumber], [Status], [UserID],
                            [City], [Country], [District], [Street], [StreetNumber],
                            [DoB], [IDCardNumber], [Note], [Sex], [StartDate], [VehicleTypeIDs] 
                    FROM Driver WHERE Id = @Id;";
                var result = conn.QueryFirstOrDefault<DriverReadModel>(sQuery, new
                {
                    Id = id
                });
                return result;
            }
        }

        public DriverReadModel GetByCode(string code)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"Select  [Id], [Code], [Name], [PhoneNumber], [Status], [UserID],
                            [City], [Country], [District], [Street], [StreetNumber],
                            [DoB], [IDCardNumber], [Note], [Sex], [StartDate], [VehicleTypeIDs] 
                    FROM Driver WHERE LOWER(Code) = LOWER(Code);";
                var result = conn.QueryFirstOrDefault<DriverReadModel>(sQuery, new
                {
                    Code = code
                });
                return result;
            }
        }

        public DriverModel Update(Driver driver)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"Update Driver  
                                        SET Name = @Name
                                        , Code =  @Code
                                        , PhoneNumber =  @PhoneNumber
                                        , Status = @Status
                                        , UserID = @UserID
                                        , StartDate = @StartDate
                                        , DoB =  @DoB
                                        , IDCardNumber = @IDCardNumber
                                        , Note = @Note
                                        , Sex = @Sex
                                        , VehicleTypeIDs = @VehicleTypeIDs
                                        , City = @City
                                        , Country = @Country
                                        , District = @District
                                        , Street = @Street
                                        , StreetNumber = @StreetNumber
                           WHERE Id = @Id;
                           SELECT @Id;";

                var result = conn.QueryFirstOrDefault<DriverModel>(sQuery, new
                {
                    Id = driver.Id.Value,
                    Name = driver.Name.Full,
                    Code = driver.Code.Value,
                    PhoneNumber = driver.PhoneNumber.Value,
                    Status = driver.Status.Value,
                    UserID = driver.UserID.Value,
                    StartDate = driver.StartDate.Value,
                    DoB = driver.DoB.Value,
                    IDCardNumber = driver.IDCardNumber.Value,
                    Note = driver.Note.Value,
                    Sex = driver.Sex.Value,
                    VehicleTypeIDs = driver.VehicleTypeIDs.Value,
                    City = driver.Address.City,
                    Country = driver.Address.Country,
                    District = driver.Address.District,
                    Street = driver.Address.Street,
                    StreetNumber = driver.Address.StreetNumber
                });
                return result;
            }
        }

        public bool ImportExcel(List<Driver> drivers)
        {
            List<DriverImportExcelModel> driversImport = new List<DriverImportExcelModel>();
            drivers.ForEach(x =>
            {
                driversImport.Add(new DriverImportExcelModel
                {
                    Code = x.Code.Value,
                    PhoneNumber = x.PhoneNumber.Value,
                    DoB = x.DoB.Value,
                    IDCardNumber = x.IDCardNumber.Value,
                    Name = x.Name.Full,
                    Sex = x.Sex.Value,
                    StartDate = x.StartDate.Value,
                    Status = x.Status.Value
                });
            });


            using (IDbConnection conn = Connection)
            {
                conn.Open();
                var trans = conn.BeginTransaction();
                // thieu volumne
                string sQuery = @"Insert Into 
                        Driver(Code, 
                                PhoneNumber,
                                DoB,
                                IDCardNumber,
                                Name,
                                Sex,
                                StartDate,
                                Status) 

                        Values( @Code,  
                                @PhoneNumber,  
                                @DoB,  
                                @IDCardNumber,  
                                @Name,  
                                @Sex,  
                                @StartDate,  
                                @Status);";

                var result = conn.Execute(sQuery, driversImport, transaction: trans);
                trans.Commit();
                Console.Write(result);
                return result > 0;
            }

        }

        public bool Delete(int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @" DELETE FROM DRIVER 
                                    WHERE Id = @Id;";
                var result = conn.Execute(sQuery, new
                {
                    Id = id
                });
                return result > 0;
            }
        }

        public DriverReadModel GetByAccount(int userId)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"Select  [Id], [Code], [Name], [PhoneNumber], [Status], [UserID],
                            [City], [Country], [District], [Street], [StreetNumber],
                            [DoB], [IDCardNumber], [Note], [Sex], [StartDate], [VehicleTypeIDs] 
                    FROM Driver WHERE UserID = @UserID;";
                var result = conn.QueryFirstOrDefault<DriverReadModel>(sQuery, new
                {
                    UserID = userId
                });
                return result;
            }
        }

        public IEnumerable<CustomerReadModel> GetCustomers(DateTime date, int driverId)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @" Select Id, Code, Name, AddressId, PhoneNumber
                                From Customer
                                WHERE id  IN (
                                SELECT CustomerId FROM dbo.RouteInfo WHERE RouterId = (
	
                                SELECT TOP 1.Id FROM dbo.Route
                                WHERE DriverID = @DriverId AND ScheduleId = (SELECT Id FROM dbo.Schedule 
                                    WHERE CAST(DeliveredAt AS DATE) = CAST(@Date AS DATE)
                                    )
                                ));";

                var result = conn.Query<CustomerReadModel>(sQuery, new
                {
                    DriverId = driverId,
                    Date = date
                });
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

        #region class is use for Insert Table Driver for Database
        private class DriverImportExcelModel
        {
            public DateTime StartDate { get; set; }
            public int Status { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Sex { get; set; }
            public DateTime DoB { get; set; }
            public string IDCardNumber { get; set; }
            public string PhoneNumber { get; set; }
        }
        #endregion
    }
}
