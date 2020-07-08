using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Matchbook.Db;
using Matchbook.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Matchbook.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderLinkingController : ControllerBase
    {
        private readonly MatchbookDbContext _context;

        public OrderLinkingController(MatchbookDbContext context)
        {
            _context = context;
        }

        /****************************TASK1**********************************/
        // POST api/<OrderLinking>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //Assumption: Input request will have 2 orders to be linked
        public async Task<IActionResult> LinkOrders([FromBody] Order orders)
        {
            //A list to hold all the orders as sent in the request
            List<Order> lstOrders = new List<Order>();
            lstOrders.Add(orders);

            if (ModelState.IsValid)
            {
                //Get Order details
                var order = await _context.Orders.Where(o => o.Id == orders.Id).AsNoTracking().SingleOrDefaultAsync();

                //Condition to check if the order is already linked to any other order
                //Checking as '0' confirming that it is not yet linked 
                if (order.LinkId == 0)
                {
                    order.Link = new OrderLink()
                    {
                        //Id is not assigned here, becuase as per OrderLink table structure it's a PK auto incremented
                        LinkedOrders = lstOrders
                    };
                    order.LinkId = orders.LinkId;

                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetOrderLinkId", new { Linkid = order.LinkId }, order);
                }
                else
                {
                    return BadRequest("The order is already linked.");
                }
            }
            else
            {
                return BadRequest("Request is not valid.");
            }
        }


        /****************************TASK2**********************************/
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //Assumption: Passing orderlink json data as request
        public async Task<IActionResult> UpdateOrderLinkName(OrderLink orderlink)
        {
            if (ModelState.IsValid)
            {
                //Get Order Link details
                var linkdetails = await _context.OrderLink.Where(o => o.Id == orderlink.Id).AsNoTracking().SingleOrDefaultAsync();

                //To check Link name is unique
                if (!_context.OrderLink.Any(e => e.Name == orderlink.Name))
                {
                    _context.OrderLink.Update(linkdetails);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("LinkName", new { LinkName = linkdetails.Name }, linkdetails);
                }
                else
                {
                    return BadRequest("Link name already exists.");
                }
            }
            else
            {
                return BadRequest("Request not in right format.");
            }
        }
    }
}
    