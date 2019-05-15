using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.Model.Entity;

namespace TransitionApp.Domain.Interface.Repository
{
    public interface ICustomerRepository
    {
        Task Create(Customer customer);
    }
}
