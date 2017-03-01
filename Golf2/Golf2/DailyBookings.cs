using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Golf2
{
    public class DailyBookings
    {
        /// <summary>
        /// ################# CONSTRUCTOR ################# 
        /// Denna klass skall instansieras. Ett datum tas emot
        /// och därefter skapas en lista för status över 
        /// hur bokningarna ser ut för aktuell dag samt
        /// en lista med alla tider som är möjliga att boka.
        /// </summary>
        public DailyBookings(DateTime date)
        {
            GetTodaysBookingTimes();
            GetTodaysBookings(date);
        }


        #region ######################## FIELDS ######################## 

        private DataTable table;
        private Booking newBooking;
        public List<Booking> BookingsPerSpecifiedDate = new List<Booking>();    // innehåller alla bokningar som gjorts en viss dag
        public List<string> AvailableBookingTimes = new List<string>();     // innehåller alla tiders om är möjliga att boka en viss dag

        #endregion

        /// <summary>
        /// Hämtar alla tider ifrån databasen som är möjliga att boka
        /// utifrån ett angivet datum. Skapar en lista med dessa tider
        /// i publika listvariabeln AvailableBookingTimes.
        /// 
        /// ########## OBS!! INJECTION EJ FIXAT ########## 
        /// 
        /// </summary>
        /// <param name="date"></param>
        private void GetTodaysBookingTimes()
        {
            string sql = "SELECT time from bookingtime WHERE active = 'true';";
            table = new DataTable();                    // skapar ny instans av table eftersom den används flera ggr i denna klass
            ToolBox.SQL_NonParam(sql, ref table);       // metoden i ToolBox uppdaterar variabeln table direkt.

            foreach (DataRow row in table.Rows)
            {
                AvailableBookingTimes.Add(Convert.ToString(row["time"]));
            }
        }

        /// <summary>
        /// Hämtar bokningar ifrån databasen utifrån 
        /// ett angivet datum. Skapar en lista med bokningar
        /// i publika list-variabeln BookingsPerSpecifiedDate.
        /// 
        /// ########## OBS!! INJECTION EJ FIXAT ########## 
        /// 
        /// </summary>
        /// <param name="date"></param>
        private void GetTodaysBookings(DateTime date)
        {
            string sql = "SELECT booking.bookingid, course.coursename, booking.bookingdate, bookingtime.time, person.golfid, person.hcp, person.gender, person.firstname, person.surname FROM PERSON ";
            sql += "RIGHT JOIN included ON person.golfid = included.golfid ";
            sql += "JOIN booking ON included.bookingid = booking.bookingid ";
            sql += "JOIN course ON booking.courseid = booking.courseid ";
            sql += "JOIN bookingtime ON bookingtime.timeid = booking.timeid ";
            sql += "WHERE booking.bookingdate = '" + date + "';";

            table = new DataTable();                    // skapar ny instans av table eftersom den används flera ggr i denna klass
            ToolBox.SQL_NonParam(sql, ref table);       // metoden i ToolBox uppdaterar variabeln table direkt.

            foreach (DataRow row in table.Rows)
            {
                newBooking = new Booking();

                newBooking.BookingId = (int)row["bookingid"];
                newBooking.CourseName = (string)row["coursename"];
                newBooking.BookingDate = Convert.ToDateTime(row["bookingdate"]); 
                newBooking.BookingTime = Convert.ToDateTime(row["time"].ToString()); 
                             
                
                newBooking.GolfId = (string)row["golfid"];
                newBooking.Gender = (string)row["gender"];
                newBooking.FirstName = (string)row["firstname"];
                newBooking.SurName = (string)row["surname"];
                newBooking.Hcp = Math.Round(Convert.ToDouble(row["hcp"]),1);

                BookingsPerSpecifiedDate.Add(newBooking);       // objekt läggs in i listan och loopen börjar om tills sista raden nåtts i datatable
            }

        }
    }
}