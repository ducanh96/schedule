﻿using System.Collections;
using System.Collections.Generic;
using TransitionApp.Domain.ReadModel.Schedule;
using TransitionApp.Domain.ReadModel.Schedule.DAO;

namespace TransitionApp.Service.Interface
{
    public interface IScheduleService
    {
        SearchScheduleReadModel GetAll(int page, int pageSize, SearchScheduleModel scheduleModel);
        ScheduleReadModel Get(int id);
        IEnumerable<RouteReadModel> GetBySchedule(int scheduleId);
    }
}