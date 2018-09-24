namespace CustomerOverview.Web.Mvc.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using CustomerOverview.Web.Common.Models;
    using CustomerOverview.Web.Mvc.Models.Customers;
    using CustomerOverview.Web.Mvc.Models.Orders;

    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;

    public class CustomersController : Controller
    {
        private const int DefaultPageSize = 10;

        private readonly HttpClient httpClient;

        public CustomersController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IActionResult> Index(int pageSize = DefaultPageSize, int page = 1)
        {
            if (page <= 0)
            {
                page = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = DefaultPageSize;
            }

            var response = await this.httpClient.GetAsync($"https://localhost:44365/api/customers?pageSize={pageSize}&page={page}");
            if (!response.IsSuccessStatusCode)
            {
                return this.RedirectToAction("Error", "Home");
            }

            var resultAsString = await response.Content.ReadAsStringAsync();
            var apiResultModel = JsonConvert.DeserializeObject<IEnumerable<CustomerBindingModel>>(resultAsString).ToList();
            var customers = apiResultModel.Select(arm => new CustomerViewModel
            {
                Id = arm.CustomerId,
                CustomerName = arm.ContactName,
                OrdersCount = arm.OrdersCount,
            });

            var pagableViewModel = new CustomersViewModel
            {
                Customers = customers,
                PageSize = pageSize,
                CurrentPage = page,
                HasNextPage = apiResultModel.Count == pageSize
            };

            return View(pagableViewModel);
        }

        public async Task<IActionResult> GetCustomersByNameContaining(string value)
        {
            var response = await this.httpClient.GetAsync($"https://localhost:44365/api/Customers/GetByCustomerNameContaining?value={value}");
            if (!response.IsSuccessStatusCode)
            {
                return this.RedirectToAction("Error", "Home");
            }

            var resultAsString = await response.Content.ReadAsStringAsync();
            var apiResultModel = JsonConvert.DeserializeObject<IEnumerable<CustomerBindingModel>>(resultAsString).ToList();
            var customers = apiResultModel.Select(arm => new CustomerViewModel
            {
                Id = arm.CustomerId,
                CustomerName = arm.ContactName,
                OrdersCount = arm.OrdersCount,
            });

            return this.PartialView("_Customers", customers);
        }

        public async Task<IActionResult> Details(string id)
        {
            var response = await this.httpClient.GetAsync($"https://localhost:44365/api/Customers/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return this.RedirectToAction("Error", "Home");
            }

            var resultAsString = await response.Content.ReadAsStringAsync();
            var apiResultModel = JsonConvert.DeserializeObject<CustomerBindingModel>(resultAsString);

            return this.View(apiResultModel);
        }

        public async Task<IActionResult> Orders(string id)
        {
            var response = await this.httpClient.GetAsync($"https://localhost:44365/api/Customers/{id}/Orders");
            if (!response.IsSuccessStatusCode)
            {
                return this.RedirectToAction("Error", "Home");
            }

            var resultAsString = await response.Content.ReadAsStringAsync();
            var apiResultModel = JsonConvert.DeserializeObject<IEnumerable<CustomerOrderBindingModel>>(resultAsString).ToList();

            var resultModel = apiResultModel.Select(co => new OrderViewModel
            {
                TotalPrice = co.OrderDetails.Sum(od => od.UnitPrice),
                TotalUnits = co.OrderDetails.Sum(od => od.Quantity),
                PossibleProblemWithExecution = co.OrderDetails.Any(od =>
                    (od.ProductDiscontinued.HasValue && od.ProductDiscontinued.Value) ||
                    od.UnitsInOrder > od.UnitsInStock)
            });

            return this.PartialView("_Orders", resultModel);
    }
}
}
