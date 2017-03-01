using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI.HtmlControls;

namespace Golf2
{
    public partial class BookingSchedule : System.Web.UI.Page
    {
        /// <summary>
        /// ######### CONSTRUCTOR ######### 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            GenerateBookingSchedule();
            
        }

        private DateTime anyDate = DateTime.Now;

        /// <summary>
        /// Bygger upp och printar ut bokningsschemat
        /// </summary>
        private void GenerateBookingSchedule()
        {
            HtmlGenericControl Schedule = new HtmlGenericControl("div");
            Schedule.Attributes.Add("id", "schedule");

            DailyBookings ShowBookings = new DailyBookings(anyDate);

            //for (int i = 0; i < 72; i++)
            //{
            //    HtmlGenericControl aDiv = new HtmlGenericControl("div");
            //    aDiv.Attributes.Add("class", "aScheduleBox");
            //    Schedule.Controls.Add(aDiv);
            //}

            foreach (string item in ShowBookings.AvailableBookingTimes)
            {
                HtmlGenericControl aBooking = new HtmlGenericControl("div");
                aBooking.Attributes.Add("class", "aBooking");
                HtmlGenericControl aBookingTime = new HtmlGenericControl("p");
                aBookingTime.InnerText = item.ToString();
                aBooking.Controls.Add(aBookingTime);
                Schedule.Controls.Add(aBooking);
            }


            DisplayBookingSchedule.Controls.Add(Schedule);          // den ihopbyggda HTML-strukturen läggs in i sidan härifrån
        }




        /// <summary>
        /// Filter för medlemsbokning
        /// </summary>
        /// <param name="bookingdate"></param>
        /// <param name="totalguests"></param>
        /// <param name="hcp"></param>
        /// <param name="hcp2"></param>
        /// <param name="hcp3"></param>
        /// <param name="hcp4"></param>
        private void MemberFilters(DateTime bookingdate, int totalguests, double hcp, double hcp2, double hcp3, double hcp4)
        {

            bool bookingvalid = BookingValid(bookingdate);
            bool bookinghcp = BookingHcp(hcp, hcp2, hcp3, hcp4);
            bool bookingguests = Guests(totalguests);

            if (bookingvalid || bookinghcp || bookingguests == true)
            {
                //ingen bokning
            }
            else
            {
                //boka tid
            }
        }

        /// <summary>
        /// Kontroll om bokningen är inom en månad
        /// </summary>
        /// <param name="bookingdate"></param>
        /// <returns>true/false</returns>
        private bool BookingValid(DateTime bookingdate)
        {
            DateTime bookingexpiry = bookingdate.AddDays(30);

            if (DateTime.Today > bookingexpiry.Date)
            {
                Session["error"] = "Tider får bokas max 30 dagar framåt";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Kontroll om totalt handicap är under 120
        /// </summary>
        /// <param name="hcp"></param>
        /// <param name="hcp2"></param>
        /// <param name="hcp3"></param>
        /// <param name="hcp4"></param>
        /// <returns>true/false</returns>
        private bool BookingHcp (double hcp, double hcp2, double hcp3, double hcp4)
        {
            int totalhcp = Convert.ToInt32(hcp + hcp2 + hcp3 + hcp4);
            int maxhcp = 120;

            if (totalhcp >= maxhcp)
            {
                Session["error"] = "För högt totalt Handicap (" + totalhcp + ")";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Kontroll om det är fler än 1 gäst i bollen
        /// </summary>
        /// <returns></returns>
        private bool Guests(int totalguests)
        {
            
            if (totalguests > 1)
            {
                Session["error"] = "Max 1 gäst per boll får anmälas";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Kontroll om någon av de anmälda redan har en tid anmäld idag
        /// </summary>
        /// <returns></returns>
        //private bool AlreadyBooked(int golfids, int totalguests)
        //{
        //    BindingList<Person> persons = new BindingList<Person>();
        //    //fyll listan från info från 

        //    golfids = golfids - totalguests;

        //    foreach (var golfid in persons)
        //    {

        //        golfids--;
        //    }
            

        //    if (golfids != 0)
        //    {
        //        Session["error"] = "Någon i sällskapet har redan en bokning samma dag";
        //        return false;
        //    }

        //    return true;


        //}

    }
}