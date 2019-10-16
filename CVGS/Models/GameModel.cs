using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVGS.Models
{
    public class GameModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string publisher { get; set; }
        public DateTime publishDate { get; set; }
        public string genre { get; set; }
        public string rating { get; set; }
        public decimal price { get; set; }

    }
}