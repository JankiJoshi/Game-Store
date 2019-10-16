using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CVGS.Models
{
    public class Event
    {
        public void eventData()
        {
            EventData = new HashSet<eventData>();
        }

        [Display(Name = "Event Number")]
        [Required]
        public int eventId { get; set; }

        [Display(Name = "Event Name")]
        [Required]
        [StringLength(50, MinimumLength = 4,
            ErrorMessage = "\n" + "Event Name must contain between 4 and 50 charaters" + "\n")]
        public string name { get; set; }

        [Display(Name = "Date of Event")]
        [Required]
        [RegularExpression(@"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])$",
            ErrorMessage = "Date of Event must be yyyy-mm-dd format" + "\n")]
        public string date { get; set; }

        [Display(Name = "Events' Creator")]
        [Required]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "Events' Creator must contain between 2 and 50 characters" + "\n")]
        public string createdBy { get; set; }

        [Display(Name = "Description")]
        [Required]
        [StringLength(8000, MinimumLength = 5,
            ErrorMessage = "Description must contain between 5 and 8000 characters" + "\n")]
        public string description { get; set; }

        public ICollection<eventData> EventData { get; set; }
    }
}