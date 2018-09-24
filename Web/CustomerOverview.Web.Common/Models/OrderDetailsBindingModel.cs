namespace CustomerOverview.Web.Common.Models
{
    public class OrderDetailsBindingModel
    {
        public double? Discount { get; set; }

        public short Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsInOrder { get; set; }

        public string ProductName { get; set; }

        public bool? ProductDiscontinued { get; set; }
    }
}
