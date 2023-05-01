using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wp13_portfolio.Models
{
    public class FavoriteBusItem
    {
        public int Id { get; set; }
        public string Gugun { get; set; }
        public string Route_no { get; set; }
        public string Starting_point { get; set; }
        public string Transfer_point { get; set; }
        public string End_point { get; set; }
        public string First_bus_time { get; set; }
        public string Last_bus_time { get; set; }
        public string Bus_interval { get; set; }
    }
}
