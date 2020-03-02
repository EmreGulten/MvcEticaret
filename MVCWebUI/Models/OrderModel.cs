using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCWebUI.Entity;

namespace MVCWebUI.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public double Total { get; set; }
        public DateTime DateTime { get; set; }
        public EnumOrderState OrderState { get; set; }
    }
}