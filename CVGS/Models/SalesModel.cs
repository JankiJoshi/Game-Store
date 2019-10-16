using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVGS.Models
{
    public class SalesModel
    {
        public string username { get; set; }
        public int orderId { get; set; }
        public decimal gameId { get; set; }
        public DateTime orderDate { get; set; }
        public DateTime shipDate { get; set; }
    }
}