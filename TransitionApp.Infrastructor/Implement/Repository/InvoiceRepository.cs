using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.ReadModel;
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

                using (var multi = conn.QueryMultiple(sQuery, new {
                    FromTime = fromTime,
                    page,
                    pageSize
                }))
                {
                    var invoices = multi.Read<InvoiceReadModel>();
                    var pageData = multi.ReadFirst<PagingReadModel>();
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

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("ConnectionStringTransition"));
            }
        }

    }
}
