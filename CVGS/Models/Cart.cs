using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVGS.Models
{
    public class Cart
    {
        //public void cartData()
        //{
        //    CartData = new HashSet<cartData>();
        //}

        public int itemId { get; set; }
        public string username { get; set; }
        public int gameId { get; set; }
        public int quantity { get; set; }

        //public ICollection<cartData> CartData { get; set; }
    }
}