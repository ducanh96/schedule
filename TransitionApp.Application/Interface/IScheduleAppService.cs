using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Application.RequestModel.Schedule;
using TransitionApp.Application.ResponseModel.Schedule;
using TransitionApp.Application.ResponseModel.Vehicle;

namespace TransitionApp.Application.Interface
{
    public interface IScheduleAppService
    {
        Task<SearchScheduleResponse> GetAll(SearchScheduleRequest request);
        Task<SearchScheduleResponse> Get(int id);
    }
}
