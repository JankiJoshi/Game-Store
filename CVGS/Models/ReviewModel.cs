using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVGS.Models
{
    public class ReviewModel
    {
        public int id { get; set; }
        public string review { get; set; }
        public decimal gameId { get; set; }
        public string username { get; set; }
        public bool approve { get; set; }
        public int rating { get; set; }
    }
}