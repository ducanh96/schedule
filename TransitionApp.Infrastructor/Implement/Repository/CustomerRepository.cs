using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.Model.Entity;

namespace TransitionApp.Infrastructor.Implement.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IConfiguration _config;
        public CustomerRepository(IConfiguration config)
        {
            _config = config;
        }

        public Task Create(Customer customer)
        {
            using (var trans = new TransactionScope())
            {
                using (IDbConnection conn = Connection)
                {
                    string sQuery =
                    @"Insert Into Address(
                                               [City]
                                             , [Country]
                                             , [District]
                                             , [Lat]
                                             , [Lng]
                                             , [Street]
                                             , [StreetNumber])

                                    Values(@City
                                        , @Country
                                        , @District
                                        , @Lat
                                        , @Lng
                                        , @Street                              
                                        , @StreetNumber); 

          SELECT Id from Address where Id = CAST(SCOPE_IDENTITY() as int)";

                    var addressId = conn.QueryFirst<int>(sQuery, new
                    {
                        City = customer.AddressCustomer.City,
                        Country = customer.AddressCustomer.Country,
                        District = customer.AddressCustomer.District,
                        Lat = customer.AddressCustomer.Lat,
                        Lng = customer.AddressCustomer.Lng,
                        Street = customer.AddressCustomer.Street,
                        StreetNumber = customer.AddressCustomer.StreetNumber
                    });

                    string queryCustomer =
                                @"Insert Into Customer(
                                                      [Code]
                                                    , [Name]
                                                    , [AddressId]
                                                    , [PhoneNumber])

                                                Values(
                                                      @Code
                                                    , @Name
                                                    , @AddressId
                                                    , @PhoneNumber);";

                    var resultInvoice = conn.Execute(queryCustomer, new
                    {
                        Code = customer.Code.Value,
                        Name = customer.Name.Full,
                        AddressId = addressId,
                        PhoneNumber = customer.PhoneNumeber.Value,
                    });

                    trans.Complete();

                };
            }
            return Task.CompletedTask;
        }

        public Task Update(Customer customer)
        {
            using (var trans = new TransactionScope())
            {
                using (IDbConnection conn = Connection)
                {

                    string sQuery =
                         @"Update Address
                          SET City = @City,
                              Country = @Country,
                              District = @District,
                              Lat = @Lat,
                              Lng = @Lng,
                              Street = @Street,
                              StreetNumber = @StreetNumber
                          WHERE Id = (
                        SELECT AddressId FROM Customer 
                        WHERE Lower(Code) = Lower(@Code)
                    );
                    Update Customer
                    Set Name = @Name,
                        PhoneNumber = @PhoneNumber
                    Where Lower(Code) = Lower(@Code);";

                    var resultInvoice = conn.Execute(sQuery, new
                    {
                        City = customer.AddressCustomer.City,
                        Country = customer.AddressCustomer.Country,
                        District = customer.AddressCustomer.District,
                        Lat = customer.AddressCustomer.Lat,
                        Lng = customer.AddressCustomer.Lng,
                        Street = customer.AddressCustomer.Street,
                        StreetNumber = customer.AddressCustomer.StreetNumber,
                        Code = customer.Code.Value,
                        Name = customer.Name.Full,
                        PhoneNumber = customer.PhoneNumeber.Value
                    });
                };
                trans.Complete();
            }
            return Task.CompletedTask;
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
