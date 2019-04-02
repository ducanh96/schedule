using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.ReadModel.Driver;

namespace TransitionApp.Service.Interface
{
    public interface IDriverService
    {
        Task<IEnumerable<DriverReadModel>> GetAll();
    }
}
