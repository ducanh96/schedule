using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransitionApp.Application.ResponseModel;

namespace TransitionApp.Models.Vehicle
{
    public class BaseResponseType:ObjectGraphType<BaseResponse>
    {
        public BaseResponseType()
        {
            Field(x => x.Code);
            Field(x => x.Message);
          
        }
    }
}
