using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TransitionApp.Domain.Interface.Repository;
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
                         [DepotLat], [DepotLng], [WarehouseId], [Distance], [ArrivalTime], [Note]
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

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("ConnectionStringTransition"));
            }
        }
    }
}
