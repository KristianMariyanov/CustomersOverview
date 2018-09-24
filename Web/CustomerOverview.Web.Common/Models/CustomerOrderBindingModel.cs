namespace CustomerOverview.Web.Common.Models
{
    using System;
    using System.Collections.Generic;

    public class CustomerOrderBindingModel
    {
        public CustomerOrderBindingModel()
        {
            this.OrderDetails = new List<OrderDetailsBindingModel>();
        }

        public int OrderId { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal? Freight { get; set; }

        public DateTime? RequiredDate { get; set; }

        public string ShipAddress { get; set; }

        public string ShipCity { get; set; }

        public string ShipCountry { get; set; }

        public string ShipPostalCode { get; set; }

        public string ShipRegion { get; set; }

        public int? ShipVia { get; set; }

        public string ShipName { get; set; }

        public DateTime? ShippedDate { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhone { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeTitle { get; set; }

        public IEnumerable<OrderDetailsBindingModel> OrderDetails { get; set; }
    }
}
