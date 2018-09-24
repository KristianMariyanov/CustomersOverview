namespace CustomerOverview.Web.Mvc.Models.Customers
{
    using System.Collections.Generic;

    public class CustomersViewModel : PagableModel
    {
        public CustomersViewModel()
        {
            this.Customers = new List<CustomerViewModel>();
        }

        public IEnumerable<CustomerViewModel> Customers { get; set; }
    }
}
