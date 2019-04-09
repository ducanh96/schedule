﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.ReadModel.Driver;
using TransitionApp.Domain.ReadModel.Driver.DAO;

namespace TransitionApp.Domain.Interface.Repository
{
    public interface IDriverRepository
    {
        #region Read
        SearchDriverReadModel GetAll(int page, int pageSize, SearchDriverModel driverModel);
        DriverReadModel Get(int id);
        #endregion

        #region Write
        DriverModel Add(Driver driver);
       
        #endregion
    }
}
