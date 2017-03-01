using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf2
{
    public class Booking
    {
        public int BookingId { get; set; }
        public string CourseName { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime BookingTime { get; set; }
        public string GolfId { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public double Hcp { get; set; }
    }
}