﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.ReadModel;
using TransitionApp.Domain.ReadModel.Vehicle;

namespace TransitionApp.Service.Interface
{
    public interface IVehicleService
    {
        Task<VehicleReadModel> Get();
        //Task<VehicleReadModel> GetById(int id);
        SearchVehicleReadModel GetAll(int page, int pageSize, SearchVehicleModel vehicleModel);
        bool checkExist(int id);
    }
}