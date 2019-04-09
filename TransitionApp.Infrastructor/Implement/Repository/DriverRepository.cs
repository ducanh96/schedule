using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.ReadModel;
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

                var result = conn.QueryFirst<DriverModel>(sQuery, new
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
                            AND (@PhoneNumber IS NULL OR PhoneNumber LIKE '@PhoneNumber%' )
                            AND (@Name IS NULL OR LOWER(Name) like LOWER('%@Name%'));

                     Select
                            [Id], [Code], [Name], [PhoneNumber], [Status], [UserID],
                            [City], [Country], [District], [Street], [StreetNumber],
                            [DoB], [IDCardNumber], [Note], [Sex], [StartDate], [VehicleTypeIDs]

                        From Driver
                        WHERE (@Code IS NULL OR LOWER(Code) = LOWER(@Code))
                        AND (@PhoneNumber IS NULL OR PhoneNumber LIKE '@PhoneNumber%' )
                        AND (@Name IS NULL OR LOWER(Name) like LOWER('%@Name%'))
                    ORDER BY Name OFFSET(@page - 1) * @pageSize ROWS FETCH NEXT @pageSize ROWS ONLY;
                    SELECT @page as Page, @pageSize as PageSize, @totalRow as Total;
                    END ";

                using (var multi = conn.QueryMultiple(sQuery, new
                {
                    page = page,
                    pageSize = pageSize,
                    Code = driverModel.Code,
                    Name = driverModel.Name,
                    PhoneNumber = driverModel.PhoneNumber
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
                var result = conn.QueryFirst<DriverReadModel>(sQuery, new
                {
                    Id = id
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
    }
}
