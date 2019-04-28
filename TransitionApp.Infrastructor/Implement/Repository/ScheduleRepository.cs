using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.ReadModel;
using TransitionApp.Domain.ReadModel.Customer;
using TransitionApp.Domain.ReadModel.Invoice;
using TransitionApp.Domain.ReadModel.Schedule;
using TransitionApp.Domain.ReadModel.Schedule.DAO;

namespace TransitionApp.Infrastructor.Implement.Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly IConfiguration _config;
        public ScheduleRepository(IConfiguration config)
        {
            _config = config;
        }

        public SearchScheduleReadModel GetAll(int page, int pageSize, SearchScheduleModel scheduleModel)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"BEGIN
                        DECLARE @totalRow INT-- tong ban ghi
                        SELECT @totalRow = Count(*) FROM dbo.Schedule
                        WHERE   (@Type IS NULL OR RouteManagerType = @Type)
                                AND (@Name IS NULL OR LOWER(Name) LIKE LOWER(@SearchName) )
                                AND (@DeliveredAt IS NULL OR CAST(DeliveredAt AS DATE) = CAST(@DeliveredAt AS DATE));

                     Select
                          [Id], [CreatedById], [DeliveredAt], [EstimatedDistance]
                            , [EstimatedDuration], [Name], [Note], [NumberOfCustomers]
                            , [RouteManagerType], [Status], [Weight], [CreatedAt]

                        From dbo.Schedule
                        WHERE (@Type IS NULL OR RouteManagerType = @Type)
                                AND (@Name IS NULL OR LOWER(Name) LIKE LOWER(@SearchName))
                                AND (@DeliveredAt IS NULL OR CAST(DeliveredAt AS DATE) = CAST(@DeliveredAt AS DATE));
                    SELECT  @page as Page, @pageSize as PageSize, @totalRow as Total;
                    END ";

                using (var multi = conn.QueryMultiple(sQuery, new
                {
                    Type = scheduleModel.Type,
                    Name = scheduleModel.Name,
                    SearchName = "%" + scheduleModel.Name + "%",
                    DeliveredAt = scheduleModel.DeliveredAt,
                    page,
                    pageSize
                }))
                {
                    var schedules = multi.Read<ScheduleReadModel>();
                    var pageData = multi.ReadFirst<PagingReadModel>();
                    SearchScheduleReadModel searchSchedule = new SearchScheduleReadModel
                    {
                        Schedules = schedules,
                        PageInfo = pageData
                    };
                    return searchSchedule;
                }
            }
        }

        public ScheduleReadModel Get(int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"
                     Select
                          [Id], [CreatedById], [DeliveredAt], [EstimatedDistance]
                            , [EstimatedDuration], [Name], [Note], [NumberOfCustomers]
                            , [RouteManagerType], [Status], [Weight], [CreatedAt]
                    From dbo.Schedule
                    WHERE Id = @Id;";
                var result = conn.QueryFirst<ScheduleReadModel>(sQuery, new
                {
                    Id = id
                });
                return result;
            }
        }

        public IEnumerable<RouteReadModel> GetRouteBySchedule(int scheduleId)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"
                     Select
                         [Id], [ScheduleId], [DriverID], [EstimatedDistance],
                         [EstimatedDuration], [Status], [Weight], [DepartureTime], [DepotAddress],
                         [DepotLat], [DepotLng], [WarehouseId], [Distance], [ArrivalTime]
                    From dbo.Route
                    WHERE ScheduleId = @Id ";
                var result = conn.Query<RouteReadModel>(sQuery, new
                {
                    Id = scheduleId
                });
                return result;
            }
        }

        public IEnumerable<RouteInfoReadModel> GetRouteInfo(int routeId)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"
                     Select [Id], [CustomerId], [RouterId], [DriverRole]
                            , [FromTime], [ToTime], [IsServed], [ServerTime]
                        
                    From dbo.RouteInfo
                    WHERE RouterId = @RouteId ";
                var result = conn.Query<RouteInfoReadModel>(sQuery, new
                {
                    RouteId = routeId
                });
                return result;
            }
        }

        public CustomerDetailReadModel GetInforCustomerOfRoute(int customerId)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"
                       Declare @AddressId int;
                     Select @AddressId = AddressId From dbo.Customer 
                             WHERE Id = @CustomerId;
                     Select [Id], [Code], [Name], [AddressId]
                             From dbo.Customer 
                             WHERE Id = @CustomerId;
            
                      Select [Id], [City], [Country], [District], [Lat], [Lng], [Street], [StreetNumber]
                            From dbo.Address
                      Where Id = @AddressId;";
                using (var multi = conn.QueryMultiple(sQuery, new
                {
                    CustomerId = customerId
                }))
                {
                    var customer = multi.ReadFirst<CustomerReadModel>();
                    var address = multi.ReadFirst<AddressReadModel>();
                    CustomerDetailReadModel searchSchedule = new CustomerDetailReadModel
                    {
                        Address = address,
                        Customer = customer
                    };
                    return searchSchedule;
                }
            }
        }

        public AddressReadModel GetInformationCustomerOfRoute(int routerInfoId)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"
                      Select [Lat], [Lng]
                            From dbo.RouteInfo
                      Where Id = @Id;";
                var result = conn.QueryFirst<AddressReadModel>(sQuery, new
                {
                    Id = routerInfoId
                });
                return result;
            }
        }

        public bool Create(Schedule schedule)
        {
           
            using (var trans = new TransactionScope())
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Open();

                    // Step 1: Insert schedule
                    string sQuery = @"Insert Into 
                                Schedule([EstimatedDistance], 
                                        [EstimatedDuration],
                                        [Name],
                                        [Note],
                                        [NumberOfCustomers],
                                        [RouteManagerType],
                                        [Status],
                                        [Weight],
                                        [DeliveredAt],
                                        [CreatedAt]) 

                                Values( @EstimatedDistance,  
                                        @EstimatedDuration,  
                                        @Name,  
                                        @Note,  
                                        @NumberOfCustomers,  
                                        @RouteManagerType,  
                                        @Status,  
                                        @Weight,
                                        @DeliveredAt,
                                        @CreatedAt
                                       );
                        SELECT Id from Schedule where Id = CAST(SCOPE_IDENTITY() as int)";

                    var resultSchedule = conn.QueryFirst<ScheduleModel>(sQuery, new
                    {
                        EstimatedDistance = schedule.EstimatedDistance.Value,
                        EstimatedDuration = schedule.EstimatedDuration.Value,
                        Name = schedule.Name.Full,
                        Note = schedule.Note.Value,
                        NumberOfCustomers = schedule.NumberOfCustomers.Value,
                        RouteManagerType = schedule.RouteManagerType.Value,
                        Status = schedule.Status.Value,
                        Weight = schedule.Weight.Value,
                        DeliveredAt = schedule.DeliveredAt.Value,
                        CreatedAt = DateTime.Now
                    });

                    // step 2: each insert route, insert list routeInfo
                    foreach (var route in schedule.Routes)
                    {
                        string queryRoute = @"Insert Into 
                                Route(
                                        [ScheduleId],
                                        [DriverID],
                                        [EstimatedDistance],
                                        [EstimatedDuration],
                                        [DepotAddress],
                                        [DepotLat],
                                        [DepotLng],
                                        [Status],
                                        [Weight]) 

                                Values( @ScheduleId,  
                                        @DriverID,  
                                        @EstimatedDistance,  
                                        @EstimatedDuration,  
                                        @DepotAddress,  
                                        @DepotLat,  
                                        @DepotLng,
                                        @Status,
                                        @Weight
                                       );
                     SELECT Id from Route where Id = CAST(SCOPE_IDENTITY() as int)";

                        var resultRoute = conn.QueryFirst<RouteModel>(queryRoute, new
                        {
                            ScheduleId = resultSchedule.Id,
                            DriverID = route.DriverID.Value,
                            EstimatedDistance = route.EstimatedDistance.Value,
                            EstimatedDuration = route.EstimatedDuration.Value,
                            DepotAddress = route.AddressDepot.FullAddress,
                            DepotLat = route.AddressDepot.Lat,
                            DepotLng = route.AddressDepot.Lng,
                            Status = route.Status.Value,
                            Weight = route.Weight.Value
                        });


                        #region Insert RouteInfo

                        string queryRouteInfo = @"Insert Into 
                                RouteInfo(
                                        [CustomerId],
                                        [RouterId],
                                        [IsServed],
                                        [Lat],
                                        [Lng],
                                        [Invoices]) 

                                Values(
                                        @CustomerId,  
                                        @RouterId,  
                                        @IsServed,  
                                        @Lat,
                                        @Lng,
                                        @Invoices
                                       );";

                        List<RouteInfoInsert> routeInfos = new List<RouteInfoInsert>();
                        foreach (var routeInfo in route.Customers)
                        {
                            RouteInfoInsert infoInsert = new RouteInfoInsert()
                            {
                                CustomerId = routeInfo.ID.Value,
                                Invoices = routeInfo.Invoices,
                                IsServed = false,
                                Lat = routeInfo.AddressCustomer.Lat,
                                Lng = routeInfo.AddressCustomer.Lng,
                                RouterId = resultRoute.Id
                            };
                            routeInfos.Add(infoInsert);
                        }

                        var resultCus = conn.Execute(queryRouteInfo, routeInfos);

                        #endregion

                    }

                    trans.Complete();
                    Console.Write(resultSchedule);
                    return resultSchedule.Id > 0;
                }


            }
           
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("ConnectionStringTransition"));
            }
        }

        private class RouteInfoInsert
        {
            public int CustomerId { get; set; }
            public int RouterId { get; set; }
            public bool IsServed { get; set; }
            public string Lat { get; set; }
            public string Lng { get; set; }
            public string Invoices { get; set; }
        }
        public enum INVOICE_STATUS
        {
            DangXuLy = 0,
            ThieuHang = 1,
            HoanThanh = 2,
            DaHuy = 3,
            DaXepLich = 4
        }

        public enum ROUTE_STATUS
        {
            ChoXacNhan = 0,
            DaXacNhan = 1,
            DangVanChuyen = 2,
            DaHoanThanh = 3,
            DaHuy = 4
        }
        public enum DRIVER_STATUS
        {
            DangLamViec = 0,
            NghiPhep = 1,
            DaNghiViec = 2
        }
    }
}
