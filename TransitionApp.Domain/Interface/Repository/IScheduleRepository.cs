﻿using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.ReadModel.Schedule;
using TransitionApp.Domain.ReadModel.Schedule.DAO;

namespace TransitionApp.Domain.Interface.Repository
{
    public interface IScheduleRepository
    {
        SearchScheduleReadModel GetAll(int page, int pageSize, SearchScheduleModel scheduleModel);
        ScheduleReadModel Get(int id);
        IEnumerable<RouteReadModel> GetBySchedule(int scheduleId);
    }
}
