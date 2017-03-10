using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf2
{
    public class Booking
    {
        public string Owner { get; set; }           // den som har utfört bokningen
        public int BookingId { get; set; }          // bokningens id
        public string CourseName { get; set; }      // banans namn
        public DateTime BookingDate { get; set; }   // datumet som bokningen gjordes för
        public DateTime BookingTime { get; set; }   // starttid
        public string GolfId { get; set; }          // spelare som bokats in (medlem eller gäst)
        public string Gender { get; set; }          // kön
        public string FirstName { get; set; }       // förnamn
        public string SurName { get; set; }         // efternamn
        public double Hcp { get; set; }             // spelarens handikapp
    }
}