namespace CustomerOverview.Tests.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using CustomerOverview.Web.Common.Models;
    using CustomerOverview.Web.Mvc.Models.Customers;

    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using Newtonsoft.Json;

    using Xunit;

    using CustomersController = CustomerOverview.Web.Mvc.Controllers.CustomersController;

    public class CustomersControllerTest
    {
        [Fact]
        public async Task AllActionShouldReturnCorrectActionResult()
        {
            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddFakeResponse(new Uri("https://localhost:44365/api/customers?pageSize=10&page=1"), await GetTestHttpClientResponseMessage());

            var httpClient = new HttpClient(fakeResponseHandler);
            var controller = new CustomersController(httpClient);

            var result = await controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task AllApiActionShouldReturnActionResultWithCorrectType()
        {
            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddFakeResponse(new Uri("https://localhost:44365/api/customers?pageSize=10&page=1"), await GetTestHttpClientResponseMessage());

            var httpClient = new HttpClient(fakeResponseHandler);
            var controller = new CustomersController(httpClient);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.IsAssignableFrom<CustomersViewModel>(viewResult.Model);
        }

        [Fact]
        public async Task AllApiActionShouldReturnCorrectCustomersCount()
        {
            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddFakeResponse(new Uri("https://localhost:44365/api/customers?pageSize=10&page=1"), await GetTestHttpClientResponseMessage());

            var httpClient = new HttpClient(fakeResponseHandler);
            var controller = new CustomersController(httpClient);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<CustomersViewModel>(viewResult.Model);

            Assert.Equal(2, model.Customers.Count());
        }

        private static Task<HttpResponseMessage> GetTestHttpClientResponseMessage()
        {
            var customers = new TestAsyncEnumerable<CustomerBindingModel>(new List<CustomerBindingModel>()
            {
                new CustomerBindingModel
                {
                    CustomerId = "ID1",
                    ContactName = "Name1",
                    ContactTitle = "Title1",
                    Address = "Address1",
                    City = "City1",
                    Country= "Country1",
                    CompanyName= "Company1",
                    Fax= "Fax1",
                    Phone= "Phone1",
                    PostalCode= "Postal1",
                    Region= "Region1",
                    OrdersCount=2,
                },
                new CustomerBindingModel
                {
                    CustomerId = "ID2",
                    ContactName = "Name2",
                    ContactTitle = "Title2",
                    Address = "Address2",
                    City = "City2",
                    Country= "Country2",
                    CompanyName= "Company2",
                    Fax= "Fax2",
                    Phone= "Phone2",
                    PostalCode= "Postal2",
                    Region= "Region2",
                    OrdersCount = 0
                }
            });

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(customers))
            };

            return Task.FromResult(response);
        }
    }
}
