using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransitionApp.Models
{
    public class TransitionAppSchema:Schema
    {
        public TransitionAppSchema(IDependencyResolver resolver): base(resolver)
        {
            Query = resolver.Resolve<TransitionAppQuery>();
            Mutation = resolver.Resolve<TransitionAppMutation>();
        }
    }
}
