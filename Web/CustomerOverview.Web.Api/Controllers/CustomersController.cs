using System.Linq;
using CustomerOverview.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOverview.Web.Api.Controllers
{
    using System.Threading.Tasks;

    using CustomerOverview.Data.Common.Repositories;
    using CustomerOverview.Web.Common.Models;

    using Microsoft.EntityFrameworkCore;

    using Remotion.Linq.Clauses;

    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private const int DefaultPageSize = 10;

        private readonly IRepository<Customers> customers;

        public CustomersController(IRepository<Customers> customers)
        {
            this.customers = customers;
        }

        [HttpGet("")]
        public async Task<IActionResult> All(int pageSize = DefaultPageSize, int page = 1)
        {
            var skip = (page - 1) * pageSize;
            var take = pageSize;

            var customerData = await this.customers
                .All()
                .Select(c => new CustomerBindingModel
                {
                    CustomerId = c.CustomerId,
                    Phone = c.Phone,
                    Fax = c.Fax,
                    Address = c.Address,
                    PostalCode = c.PostalCode,
                    Country = c.Country,
                    Region = c.Region,
                    City = c.City,
                    CompanyName = c.CompanyName,
                    ContactName = c.ContactName,
                    ContactTitle = c.ContactTitle,
                    OrdersCount = c.Orders.Count
                })
                .OrderBy(c => c.CustomerId)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return new JsonResult(customerData);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetByCustomerNameContaining(string value)
        {
            var customerData = await this.customers
                .All()
                .Where(c => c.ContactName.Contains(value))
                .Select(c => new CustomerBindingModel
                {
                    CustomerId = c.CustomerId,
                    Phone = c.Phone,
                    Fax = c.Fax,
                    Address = c.Address,
                    PostalCode = c.PostalCode,
                    Country = c.Country,
                    Region = c.Region,
                    City = c.City,
                    CompanyName = c.CompanyName,
                    ContactName = c.ContactName,
                    ContactTitle = c.ContactTitle,
                    OrdersCount = c.Orders.Count
                })
                .OrderBy(c => c.CustomerId)
                .Take(DefaultPageSize)
                .ToListAsync();

            return new JsonResult(customerData);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(string id)
        {
            var customerDetails = await this.customers
                .GetByIdQueryable(id)
                .Select(c => new CustomerBindingModel
                {
                    CustomerId = c.CustomerId,
                    Phone = c.Phone,
                    Fax = c.Fax,
                    Address = c.Address,
                    PostalCode = c.PostalCode,
                    Country = c.Country,
                    Region = c.Region,
                    City = c.City,
                    CompanyName = c.CompanyName,
                    ContactName = c.ContactName,
                    ContactTitle = c.ContactTitle,
                })
                .FirstOrDefaultAsync();

            if (customerDetails == null)
            {
                return this.NotFound("Customer not found.");
            }

            return new JsonResult(customerDetails);
        }

        [HttpGet("{id}/orders")]
        public async Task<IActionResult> Orders(string id)
        {
            var customerData = await this.customers
                .GetByIdQueryable(id)
                .Select(c => new
                {
                    c.CustomerId,
                    Orders = c.Orders.Select(o => new CustomerOrderBindingModel
                    {
                        OrderId = o.OrderId,
                        OrderDetails = o.OrderDetails
                            .Select(od => new OrderDetailsBindingModel
                            {
                                Discount = od.Discount,
                                Quantity = od.Quantity,
                                UnitsInStock = od.Product.UnitsInStock,
                                UnitsInOrder = od.Product.UnitsOnOrder,
                                UnitPrice = od.UnitPrice,
                                ProductName = od.Product.ProductName,
                                ProductDiscontinued = od.Product.Discontinued
                            }),
                        OrderDate = o.OrderDate,
                        Freight = o.Freight,
                        RequiredDate = o.RequiredDate,
                        ShipAddress = o.ShipAddress,
                        ShipCity = o.ShipCity,
                        ShipCountry = o.ShipCountry,
                        ShipPostalCode = o.ShipPostalCode,
                        ShipRegion = o.ShipRegion,
                        ShipVia = o.ShipVia,
                        ShipName = o.ShipName,
                        ShippedDate = o.ShippedDate,
                        CustomerName = o.Customer.ContactName,
                        CustomerPhone = o.Customer.Phone,
                        EmployeeName = o.Employee.FirstName + " " + o.Employee.LastName,
                        EmployeeTitle = o.Employee.Title
                    })
                })
                .FirstOrDefaultAsync();

            if (customerData == null)
            {
                return this.NotFound("Customer not found.");
            }

            return new JsonResult(customerData.Orders);
        }
    }
}
