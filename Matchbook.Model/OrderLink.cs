using System;
using System.Collections.Generic;
using System.Text;

namespace Matchbook.Model
{
    public class OrderLink
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public ICollection<Order> LinkedOrders { get; set; }
    }
}
