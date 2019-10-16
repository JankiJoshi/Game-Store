using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVGS.Models
{
    public class CreditInformationsModel
    {
        public int cardNumber { get; set; }
        public string cardHolder { get; set; }
        public string cardType { get; set; }
        public string username { get; set; }
        public string expiryDate { get; set; }
        public int securityCode { get; set; }
    }
}