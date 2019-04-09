using System.Collections.Generic;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.ReadModel.Schedule;
using TransitionApp.Domain.ReadModel.Schedule.DAO;
using TransitionApp.Service.Interface;

namespace TransitionApp.Service.Implement
{
    public class ScheduleService : IScheduleService
    {
        public readonly IScheduleRepository _scheduleRepository;
        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public ScheduleReadModel Get(int id)
        {
            return _scheduleRepository.Get(id);
        }

        public SearchScheduleReadModel GetAll(int page, int pageSize, SearchScheduleModel scheduleModel)
        {
            return _scheduleRepository.GetAll(page, pageSize, scheduleModel);
        }

        public IEnumerable<RouteReadModel> GetBySchedule(int scheduleId)
        {
            return _scheduleRepository.GetBySchedule(scheduleId);
        }
    }
}
