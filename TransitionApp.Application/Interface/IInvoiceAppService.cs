using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Application.RequestModel.Invoice;
using TransitionApp.Application.ResponseModel.Invoice;

namespace TransitionApp.Application.Interface
{
    public interface IInvoiceAppService
    {
        Task<SearchInvoiceResponse> GetAll(FilterInvoiceRequest request);
    }
}
