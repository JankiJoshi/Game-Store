//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CVGS
{
    using System;
    using System.Collections.Generic;
    
    public partial class review
    {
        public int id { get; set; }
        public string review1 { get; set; }
        public decimal gameId { get; set; }
        public string username { get; set; }
        public Nullable<bool> approve { get; set; }
        public Nullable<decimal> rating { get; set; }
    
        public virtual game game { get; set; }
    }
}
