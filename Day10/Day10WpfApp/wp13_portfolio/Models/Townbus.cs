using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace wp13_portfolio.Models
{
    public class Townbus
    {
        /*
        "route_no": "연제1",
        "starting_point": "물만골",
        "transfer_point": "연산전화국",
        "end_point": "시청역",      
        "num_of_vehicles": "2",
        "num_of_spare_vehicles": "1",
        "first_bus_time": "6:00",
        "last_bus_time": "23:15",
        "bus_interval": "15",
        "interval_rush_hours": "15",
        "interval_weekend": "15",
        "general_card_fare": "1130",
        "general_cash_fare": "1200",
        "teen_card_fare": "750",
        "teen_cash_fare": "900",
        "child_card_fare": "260",
        "child_cash_fare": "300",
        "gugun": "연제구",
        "reference_date": "2022-12-31" */
        public int Id { get; set; } 
        public string Route_no { get; set; }
        public string Starting_point { get; set; }
        public string Transfer_point { get; set; }
        public string End_point { get; set;}
        public string First_bus_time { get; set; }
        public string Last_bus_time { get; set; }
        public string Bus_interval { get; set; }
        public string Gugun { get; set; }
    }
}
