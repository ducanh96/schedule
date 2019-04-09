using System;
using System.Linq;
using System.Threading.Tasks;
using TransitionApp.Application.Interface;
using TransitionApp.Application.RequestModel.Invoice;
using TransitionApp.Application.ResponseModel.Invoice;
using TransitionApp.Service.Interface;

namespace TransitionApp.Application.Implement
{
    public class InvoiceAppService : IInvoiceAppService
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceAppService(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }
        public Task<SearchInvoiceResponse> GetAll(FilterInvoiceRequest request)
        {
            var result = new SearchInvoiceResponse();
            try
            {
                var invoiceAll = _invoiceService.GetAll(request.page, request.page_size, request.from_time ?? null);
                var invoicesData = from a in invoiceAll.Invoices
                                   let customer = _invoiceService.GetCustomer(a.CustomerId)
                                   let address = _invoiceService.GetAddress(customer.AddressId)
                                   let item = _invoiceService.GetItems(a.Id)
                                   select new
                                   {
                                       ID = a.Id,
                                       //a.DeliveryTime,
                                       a.Note,
                                       a.Served,
                                       //a.ServerTime,
                                       a.Status,
                                       a.TotalPrice,
                                       a.WeightTotal,
                                       DeliveryTime = a.DeliveryTime,

                                       TimeWindows = new
                                       {
                                           FromTime = 0,
                                           ToTime = 86340
                                       },

                                       CustomerCode = customer.Code,
                                       CustomerName = customer.Name,
                                       CustomerID = customer.Id.ToString(),
                                       Address = new
                                       {
                                           address.City,
                                           address.Country,
                                           address.District,
                                           address.Id,
                                           address.StreetNumber,
                                           address.Street,
                                           Lat = double.Parse(address.Lat),
                                           Lng = double.Parse(address.Lng)
                                       },
                                       Items = item

                                   };
                result.Data = invoicesData.ToList();
                result.Total = invoiceAll.PageInfo.Total;
            }
            catch (Exception ex)
            {
                result.Total = 0;
                result.Message = ex.Message;

            }
            return Task.FromResult(result);

        }
    }
}
