using Matchbook.Db;
using Matchbook.Model;
using Matchbook.WebHost.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;
using Xunit;

namespace OrderLinkUnitTestProject
{
    public class OrderLinkTest
    {
        private readonly MatchbookDbContext _context;

        public OrderLinkTest(MatchbookDbContext context)
        {
            _context = context;
        }



        [Fact]
        public void LinkOrders()
        {
            var controller = new OrderLinkingController(_context);

            //A list to hold all the orders as sent in the request
            string jsonString;
            jsonString = "{'Id': 1, 'SubAccountId': 1},{'Id': 2, 'SubAccountId': 2}";

            // Act     
            Order orders = Newtonsoft.Json.JsonConvert.DeserializeObject<Order>(jsonString);
            var response = controller.LinkOrders(orders) as IActionResult;
            _context.Dispose();

            // Assert
            Assert.False(false);

        }

        [Fact]
        public void LinkNameShouldBeUnique()
        {
            //Get a sample order from the DB
            var existingOrderLink = _context.OrderLink.Where(o => o.Id == 1).SingleOrDefault();

            //Act
            OrderLink newOrderLink = new OrderLink();
            newOrderLink.Name = existingOrderLink.Name;
            _context.OrderLink.Add(newOrderLink);

            //Assert
            Assert.Throws<DbUpdateException>(() => _context.SaveChanges());
        }
    }
}
