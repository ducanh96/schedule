using GraphQL.Instrumentation;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransitionApp.Domain.ReadModel;
using TransitionApp.Domain.ReadModel.Driver;

namespace TransitionApp.Models
{
    public class DriverType: ObjectGraphType<DriverReadModel>
    {
        public DriverType()
        {
            Field(x => x.Id);
           
            
        }
    }
}
