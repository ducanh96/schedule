﻿using MassTransit;
using OrderService.Domain.Commands.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.ReadModel.Customer;
using TransitionApp.Domain.Model.ValueObject;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.ReadModel.Invoice;

namespace TransitionApp.Domain.Consumer.Invoice
{
    public class InvoiceConsumer : IConsumer<AddNewInvoiceCommand>, IConsumer<UpdateInvoiceCommand>, IConsumer<CancelInvoiceModel>
    {
        public IInvoiceRepository _invoiceRepository;

        public InvoiceConsumer(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }


        public Task Consume(ConsumeContext<AddNewInvoiceCommand> context)
        {
            try
            {
                AddNewInvoiceCommand newInvoice = context.Message;
                CustomerReadModel customerRead = _invoiceRepository.GetCustomer(newInvoice.CustomerCode);
                Model.Entity.Invoice invoice;
                List<Item> items = new List<Item>();
                newInvoice.Items.ToList().ForEach(x =>
                {
                    Item item = new Item(
                        new IsDeliveried(x.Deliveried),
                        new Quantity(x.DeliveriedQuantity),
                        new Price(x.Price),
                        new Name(x.ProductName),
                        new Quantity(x.Quantity),
                        new Price(x.TotalPrice),
                        new Unit(x.UnitName),
                        new Weight(Convert.ToDouble(x.Weight))
                        );
                    items.Add(item);
                });
                if (customerRead != null)
                {
                    invoice = new Model.Entity.Invoice(
                    new Code(newInvoice.Code),
                    new Note(newInvoice.Note),
                    new Status(newInvoice.Status),
                    new Price(newInvoice.TotalPrice),
                    new Weight(newInvoice.WeightTotal),
                    new Identity(customerRead.Id),
                    newInvoice.DeliveryTime,
                    new IsServed(newInvoice.Served),
                    items
                    );
                }
                else
                {
                    return Task.FromException(new Exception("Lỗi dữ liệu"));
                }



                return _invoiceRepository.Create(invoice);

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }

        }

        public Task Consume(ConsumeContext<UpdateInvoiceCommand> context)
        {
            try
            {
                UpdateInvoiceCommand updateInvoice = context.Message;
                List<Item> items = new List<Item>();
                updateInvoice.Items.ToList().ForEach(x =>
                {
                    Item item = new Item(
                        new IsDeliveried(x.Deliveried),
                        new Quantity(x.DeliveriedQuantity),
                        new Price(x.Price),
                        new Name(x.ProductName),
                        new Quantity(x.Quantity),
                        new Price(x.TotalPrice),
                        new Unit(x.UnitName),
                        new Weight(Convert.ToDouble(x.Weight))
                        );
                    items.Add(item);
                });
                Model.Entity.Invoice invoice = new Model.Entity.Invoice(
                    new Code(updateInvoice.Code),
                    new Price(updateInvoice.TotalPrice),
                    new Weight(updateInvoice.WeightTotal),
                    items);

                return _invoiceRepository.Update(invoice);

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }

        }

        public Task Consume(ConsumeContext<CancelInvoiceModel> context)
        {
            try
            {
                CancelInvoiceModel cancelInvoice = context.Message;
                Model.Entity.Invoice invoice = new Model.Entity.Invoice(
                 new Code(cancelInvoice.Code),
                 new Status(cancelInvoice.Status)
                 );
                return _invoiceRepository.UpdateInvoiceStatus(invoice);

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return Task.CompletedTask;
            }
            

        }
    }
}
