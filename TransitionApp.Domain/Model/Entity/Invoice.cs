using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.Model.ValueObject;

namespace TransitionApp.Domain.Model.Entity
{
    public class Invoice
    {
        public Invoice(Code code, Price totalPrice, Weight weightTotal, List<Item> items)
        {
            Code = code;
            TotalPrice = totalPrice;
            WeightTotal = weightTotal;
            Items = items;
        }

        public Invoice(Code code, Note note, Status status,
            Price totalPrice, Weight weightTotal, Identity customerId,
            DateTime deliveryTime, IsServed served, List<Item> items)
        {
            Code = code;
            Note = note;
            Status = status;
            TotalPrice = totalPrice;
            WeightTotal = weightTotal;
            CustomerId = customerId;
            DeliveryTime = deliveryTime;
            Served = served;
            Items = items;
        }

        public Code Code { get; set; }
        public Note Note { get; set; }
        public Status Status { get; set; }
        public Price TotalPrice { get; set; }
        public Weight WeightTotal { get; set; }
        public Identity CustomerId { get; set; }
        public DateTime DeliveryTime { get; set; }
        public IsServed Served { get; set; }
        public List<Item> Items { get; set; }
      


    }

    public class Item
    {
        public Item(IsDeliveried deliveried, Quantity deliveriedQuantity, Price price,
            Name productName, Quantity quantity, Price totalPrice, Unit unitName, Weight weight)
        {
            Deliveried = deliveried;
            DeliveriedQuantity = deliveriedQuantity;
            Price = price;
            ProductName = productName;
            Quantity = quantity;
            TotalPrice = totalPrice;
            UnitName = unitName;
            Weight = weight;
        }

        public IsDeliveried Deliveried { get; set; }
        public Quantity DeliveriedQuantity { get; set; }
        public Price Price { get; set; }
        public Name ProductName { get; set; }
        public Quantity Quantity { get; set; }
        public Price TotalPrice { get; set; }
        public Unit UnitName { get; set; }
        public Weight Weight { get; set; }
        

    }
}
