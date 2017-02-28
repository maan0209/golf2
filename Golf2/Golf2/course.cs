using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf2
{
   

    public class course
    {
        public int CourseId { get; set; }
        public string Coursename { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public bool Closed { get; set; }

    }

}