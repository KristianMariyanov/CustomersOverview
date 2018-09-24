namespace CustomerOverview.Tests.Web.Api
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CustomerOverview.Data.Common.Repositories;
    using CustomerOverview.Data.Models;
    using CustomerOverview.Web.Common.Models;

    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using Xunit;

    using CustomersController = CustomerOverview.Web.Api.Controllers.CustomersController;

    public class CustomersControllerTest
    {
        [Fact]
        public async Task AllApiActionShouldReturnCorrectActionResult()
        {
            var mockRepo = new Mock<IRepository<Customers>>();
            mockRepo.Setup(repo => repo.All())
                .Returns(GetTestCustomers());
            var controller = new CustomersController(mockRepo.Object);

            var result = await controller.All();

            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task AllApiActionShouldReturnActionResultWithCorrectType()
        {
            var mockRepo = new Mock<IRepository<Customers>>();
            mockRepo.Setup(repo => repo.All())
                .Returns(GetTestCustomers());
            var controller = new CustomersController(mockRepo.Object);

            var result = await controller.All();

            var jsonResult = Assert.IsType<JsonResult>(result);

            Assert.IsAssignableFrom<IEnumerable<CustomerBindingModel>>(
                jsonResult.Value);
        }

        [Fact]
        public async Task AllApiActionShouldReturnCorrectElementsCount()
        {
            var mockRepo = new Mock<IRepository<Customers>>();
            mockRepo.Setup(repo => repo.All())
                .Returns(GetTestCustomers());
            var controller = new CustomersController(mockRepo.Object);

            var result = await controller.All();

            var jsonResult = Assert.IsType<JsonResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<CustomerBindingModel>>(
                jsonResult.Value);

            Assert.Equal(2, model.Count());
        }

        private static IQueryable<Customers> GetTestCustomers()
        {
            var customers = new TestAsyncEnumerable<Customers>(new List<Customers>()
            {
                new Customers
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
                    Orders= new List<Orders>()
                    {
                        new Orders(),
                        new Orders()
                    },
                },
                new Customers
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
                    Orders= new List<Orders>()
                    {
                        new Orders()
                    },
                }
            });

            return customers.AsQueryable();
        }
    }
}
