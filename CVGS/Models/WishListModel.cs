using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVGS.Models
{
    public class WishListModel
    {
        public string username { get; set; }
        public int gameId { get; set; }
        public DateTime dateAdded { get; set; }

    }
}