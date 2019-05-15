using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.ReadModel;
using TransitionApp.Domain.ReadModel.Customer;
using TransitionApp.Domain.ReadModel.Invoice;

namespace TransitionApp.Infrastructor.Implement.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IConfiguration _config;
        public InvoiceRepository(IConfiguration config)
        {
            _config = config;
        }

        public SearchInvoiceReadModel GetAll(int page, int pageSize, DateTime? fromTime)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"BEGIN
                        DECLARE @totalRow INT-- tong ban ghi
                        SELECT @totalRow = Count(*) FROM dbo.Invoice
                        WHERE @FromTime IS NULL OR CAST(DeliveryTime AS DATE) = CAST(@FromTime AS DATE);

                     Select
                           [Id], [Note], [Served], [ServerTime], [Status],
                           [FromTime], [ToTime], [DeliveryTime], [TotalPrice]
                            , [WeightTotal], [CustomerId]

                        From Invoice
                        WHERE @FromTime IS NULL OR CAST(DeliveryTime AS DATE) = CAST(@FromTime AS DATE);
                    SELECT  @page as Page, @pageSize as PageSize, @totalRow as Total;
                    END ";

                using (var multi = conn.QueryMultiple(sQuery, new
                {
                    FromTime = fromTime,
                    page,
                    pageSize
                }))
                {
                    var invoices = multi.Read<InvoiceReadModel>();
                    var pageData = multi.ReadFirstOrDefault<PagingReadModel>();
                    SearchInvoiceReadModel searchVehicle = new SearchInvoiceReadModel
                    {
                        Invoices = invoices,
                        PageInfo = pageData
                    };
                    return searchVehicle;
                }

            }
        }

        public CustomerReadModel GetCustomer(int customerId)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @" Select Id, Code, Name, AddressId
                        From Customer Where Id = @Id";
                var result = conn.QueryFirstOrDefault<CustomerReadModel>(sQuery, new
                {
                    Id = customerId
                });
                return result;
            }
        }

        public AddressReadModel GetAddress(int addressId)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @" Select [Id], [City], [Country], 
                [District], [Lat], [Lng], [Street], [StreetNumber] From Address
                 Where Id = @Id   ";
                var result = conn.QueryFirstOrDefault<AddressReadModel>(sQuery, new
                {
                    Id = addressId
                });
                return result;
            }
        }

        public IEnumerable<ItemReadModel> GetItems(int invoiceId)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"SELECT [Id], [Deliveried], [DeliveriedQuantity]
                                , [IsExistedProduct], [Note], [Price]
                                , [ProductID], [ProductName], [Quantity]
                                , [TotalPrice], [UnitID],[UnitName]
                                , [Weight], [InvoiceId]
                 FROM Item
                 Where InvoiceId = @InvoiceId   ";
                var result = conn.Query<ItemReadModel>(sQuery, new
                {
                    InvoiceId = invoiceId
                });
                return result;
            }
        }

        public Task Create(Invoice invoice)
        {
            using (var trans = new TransactionScope())
            {
                using (IDbConnection conn = Connection)
                {
                    string sQuery =
                        @"Insert Into Invoice( Code
                                            , Note
                                            , Status
                                            , TotalPrice
                                            , WeightTotal
                                            , CustomerId
                                            , DeliveryTime
                                            , Served)

                                Values(@Code
                                    , @Note
                                    , @Status
                                    , @TotalPrice
                                    , @WeightTotal
                                    , @CustomerId                              
                                    , @DeliveryTime
                                    , @Served); 

          SELECT Id from Invoice where Id = CAST(SCOPE_IDENTITY() as int)";


                    var resultInvoice = conn.QueryFirst<int>(sQuery, new
                    {
                        Code = invoice.Code.Value,
                        Note = invoice.Note.Value,
                        Status = invoice.Status.Value,
                        TotalPrice = invoice.TotalPrice.Value,
                        WeightTotal = invoice.WeightTotal.Value,
                        CustomerId = invoice.CustomerId.Value,
                        DeliveryTime = invoice.DeliveryTime,
                        Served = invoice.Served.Value
                    });

                    string queryItem =
                                  @"Insert Into Item( Deliveried
                                        , Price
                                        , ProductName
                                        , TotalPrice
                                        , UnitName
                                        , Quantity
                                        , Weight
                                        , InvoiceId)

                                Values(@Deliveried
                                    , @Price
                                    , @ProductName
                                    , @TotalPrice
                                    , @UnitName                              
                                    , @Quantity
                                    , @Weight
                                     ,@InvoiceId); ";

                    List<InsertItemModel> insertItems = new List<InsertItemModel>();
                    invoice.Items.ForEach(x =>
                    {
                        InsertItemModel insertItem = new InsertItemModel
                        {
                            Deliveried = x.Deliveried.Value,
                            Price = x.Price.Value,
                            ProductName = x.ProductName.Full,
                            Quantity = x.Quantity.Value,
                            TotalPrice = x.TotalPrice.Value,
                            UnitName = x.UnitName.Value,
                            Weight = x.Weight.Value,
                            InvoiceId = resultInvoice
                        };
                        insertItems.Add(insertItem);
                    });

                    conn.Execute(queryItem, insertItems);
                    trans.Complete();

                }

                return Task.CompletedTask;
            }
        }

        public CustomerReadModel GetCustomer(string customerCode)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @" Select Id, Code, Name, AddressId
                        From Customer Where Lower(Code) = Lower(@Code);";
                var result = conn.QueryFirstOrDefault<CustomerReadModel>(sQuery, new
                {
                    Code = customerCode
                });
                return result;
            }
        }

        public IEnumerable<InvoiceReadModel> GetInvoices(DateTime deliverTime, int driverId, int customerId)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @" Select Id, [Note], [Served], [ServerTime], [Status], [DeliveryTime]
                                , [TotalPrice], [WeightTotal], [CustomerId], [Code]
                                FROM Invoice  WHERE id
                                    IN (
                                        SELECT Invoices FROM dbo.RouteInfo WHERE CustomerId = @CustomerId And RouterId = (
                                        SELECT TOP 1.Id FROM dbo.Route
                                        WHERE DriverID = @DriverId AND ScheduleId = (SELECT Id FROM dbo.Schedule 
                                            WHERE CAST(DeliveredAt AS DATE) = CAST(@DeliverTime AS DATE)
                                            )
                                        ));";

                var result = conn.Query<InvoiceReadModel>(sQuery, new
                {
                    DriverId = driverId,
                    DeliverTime = deliverTime,
                    CustomerId = customerId
                });
                return result;
            }
        }

        public InvoiceReadModel UpdateVoice(int invoiceId, int status)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"Update Invoice
                               SET Status = @Status 
                               WHERE Id = @Id;
                SELECT Code, Status, Served FROM INVOICE
                WHERE Id = @Id;";

                var result = conn.QueryFirst<InvoiceReadModel>(sQuery, new
                {
                    Status = status,
                    Id = invoiceId
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

        public class InsertItemModel
        {
            public bool Deliveried { get; set; } = false;
            public double Price { get; set; }
            public string ProductName { get; set; }
            public double TotalPrice { get; set; }
            public string UnitName { get; set; }
            public int Quantity { get; set; }
            public double Weight { get; set; }
            public int InvoiceId { get; set; }
        }

    }
}
