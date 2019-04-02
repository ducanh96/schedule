using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.ReadModel.Driver;

namespace TransitionApp.Domain.Interface.Repository
{
    public interface IDriverRepository
    {
        #region Read
        Task<IEnumerable<DriverReadModel>> GetAll();
        #endregion

        #region Write
        Task Add(Driver driver);
        #endregion
    }
}
