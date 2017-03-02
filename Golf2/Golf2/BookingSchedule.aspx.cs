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

        #region ########## FIELDS ########## 

        private DateTime anyDate = DateTime.Now;

        #endregion

        /// <summary>
        /// Bygger upp och printar ut bokningsschemat till sidan
        /// </summary>
        private void GenerateBookingSchedule()
        {
            HtmlGenericControl Schedule = new HtmlGenericControl("div");            // behållare för schema-content skapas här
            Schedule.Attributes.Add("id", "schedule");

            DailyBookings ShowBookings = new DailyBookings(anyDate);                // "dagens" bokningar + bokningsbara tider hämtas

            foreach (string aBookingTime in ShowBookings.AvailableBookingTimes)     // loopar genom tillgängliga bokningstider
            {
                HtmlGenericControl aBooking = new HtmlGenericControl("div");

                CreateButtonContent(ref aBooking, aBookingTime, ShowBookings.BookingsPerSpecifiedDate);
                
                Schedule.Controls.Add(aBooking);
            }

            DisplayBookingSchedule.Controls.Add(Schedule);                          // den ihopbyggda HTML-strukturen läggs in i sidan
        }

        /// <summary>
        /// Lägger till boknings-content till en HTML-div 
        /// för en viss tidpunkt i det aktuella bokningsschemat
        /// </summary>
        /// <param name="aBooking"></param>
        /// <param name="aBookingTime"></param>
        /// <param name="bookingsPerSpecifiedDate"></param>
        private void CreateButtonContent(ref HtmlGenericControl aBooking, string aBookingTime, List<Booking> bookingsPerSpecifiedDate)
        {
            DateTime convTime = Convert.ToDateTime(aBookingTime);
            aBooking.Attributes.Add("id", convTime.ToShortTimeString());
            // ############## Bootstrapkod för att trigga Modal (ej aBooking) 
            aBooking.Attributes.Add("class", "aBooking span4 proj-div");
            aBooking.Attributes.Add("data-toggle", "modal");
            aBooking.Attributes.Add("data-target", "#"+convTime.ToShortTimeString());

            // ############## Info om vilket klockslag en viss bokning gäller
            HtmlGenericControl textInfo = new HtmlGenericControl("p");
            textInfo.InnerText = convTime.ToShortTimeString();
            aBooking.Controls.Add(textInfo);

            // ############## Bootstrapkod för vad som ska visas i Modal 
            HtmlGenericControl lvl01 = new HtmlGenericControl("div");
            lvl01.Attributes.Add("id", convTime.ToShortTimeString());
            //lvl01.Attributes.Add("");
            //lvl01.Attributes.Add("");

            int counter = 0;
            foreach (Booking item in bookingsPerSpecifiedDate)                                  // loopa genom de bokningar som finns för dagen
            {
                if (item.BookingTime.ToShortTimeString() == convTime.ToShortTimeString())       // reglerar att enbart aktuell tid (se aBookingTime) behandlas
                {
                    counter++;
                    // bygg upp bodyn 
                }
                if (counter == 0)
                {
                    // visa text att inget är bokat
                }
            }

            /* ######## HTML-KODEXEMPEL FÖR BOOTSTRAP MODAL ######## 
             * Nedan kod går att köra direkt i ett HTML-dokument. Används som mall för att bygga strukturen ovan.
             * 
             * <div class="span4 proj-div" data-toggle="modal" data-target="#GSCCModal"> asdfasd</div>
             *
             * ######## MODAL-KOD FÖR POPUP-FÖNSTRET ##########
             * lvl01 ---> <div id="GSCCModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
             *      <div class="modal-dialog">
             *          <div class="modal-content">
             *              <div class="modal-header">
             *                  <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;  </button>
             *                  <h4 class="modal-title" id="myModalLabel">Modal title</h4>
             *              </div>
             *              <div class="modal-body">
             *              ...
             *              </div>
             *              <div class="modal-footer">
             *                  <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
             *                  <button type="button" class="btn btn-primary">Save changes</button>
             *              </div>
             *          </div>
             *      </div>
             *  </div>
             *  Källa: http://stackoverflow.com/questions/20111219/bootstrap-modals-how-to-open-with-onclick
             */
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