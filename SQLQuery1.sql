SELECT * FROM dbo.Route
WHERE ScheduleId = (SELECT Id FROM dbo.Schedule 
WHERE CAST(DeliveredAt AS DATE) = CAST('4-24-2019' AS DATE)
) AND DriverID = 1

SELECT * FROM dbo.RouteInfo
INNER JOIN dbo.Route
ON Route.Id = RouteInfo.RouterId

SELECT * FROM Customer 
WHERE Id = (SELECT dbo.RouteInfo.CustomerId FROM dbo.RouteInfo 
			WHERE RouterId = 
				(SELECT Id  FROM dbo.Route WHERE DriverID = 1 AND ScheduleId = 
					(SELECT Id FROM dbo.Schedule WHERE 1 = 1 ) 
				)
				)

SELECT * FROM dbo.Customer
WHERE id  IN (

SELECT CustomerId FROM dbo.RouteInfo WHERE RouterId = (

	SELECT TOP 1.Id FROM dbo.Route
WHERE ScheduleId = (SELECT Id FROM dbo.Schedule 
WHERE CAST(DeliveredAt AS DATE) = CAST('4-24-2019' AS DATE)
) AND DriverID = 1

)

)