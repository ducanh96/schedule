﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Application.RequestModel.Driver;
using TransitionApp.Application.ResponseModel.Driver;

namespace TransitionApp.Application.Interface
{
    public interface IDriverAppService
    {
        #region Read
        Task<DriverResponse> GetAll(SearchDriverRequest request);
        Task<GetDriverResponse> Get(int id);
        #endregion
        #region Write
        Task<CreateDriverResponse> Create(CreateDriverRequest request);
        #endregion
    }
}
