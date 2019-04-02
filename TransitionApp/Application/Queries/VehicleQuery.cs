using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TransitionApp.Application.Queries
{
    public class VehicleQuery : IVehicleQueries
    {
        public async Task<List<Vehicle>> GetAll()
        {
            // add Dapper to query


            //fake data
            var vehicleAll = await Task.Run(() =>
            {
                return new List<Vehicle>
                {
                    new Vehicle
                    {
                        Id = 1,
                        Name = "Honda"
                    },
                     new Vehicle
                    {
                        Id = 2,
                        Name = "Way"
                    }
                };
            });

            return vehicleAll;
        }
    }
}
