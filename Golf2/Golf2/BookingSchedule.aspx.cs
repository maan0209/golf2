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
        int co = 0;
        #endregion

        /// <summary>
        /// Bygger upp och printar ut bokningsschemat till sidan
        /// </summary>
        private void GenerateBookingSchedule()
        {
            HtmlGenericControl Schedule = new HtmlGenericControl("div");            // behållare för schema-content skapas här
            Schedule.Attributes.Add("id", "schedule");

            DailyBookings ShowBookings = new DailyBookings(anyDate);                // "dagens" bokningar + bokningsbara tider hämtas

            HtmlGenericControl newModals = new HtmlGenericControl("div");
            newModals.Attributes.Add("id", "newModals");

            foreach (string aBookingTime in ShowBookings.AvailableBookingTimes)     // loopar genom tillgängliga bokningstider
            {
                HtmlGenericControl aBooking = new HtmlGenericControl("div");

                CreateButtonContent(ref aBooking, aBookingTime, ShowBookings.BookingsPerSpecifiedDate);

                CreateModalPopups(aBookingTime, ShowBookings.BookingsPerSpecifiedDate, ref newModals);

                Schedule.Controls.Add(aBooking);
            }



            DisplayBookingSchedule.Controls.Add(Schedule);                          // den ihopbyggda HTML-strukturen läggs in i sidan
            DisplayBookingSchedule.Controls.Add(newModals);
        }

        /// <summary>
        /// Skapar content i modals popuperna och visar 
        /// statusen för bokningstiden
        /// </summary>
        /// <param name="aBookingTime"></param>
        /// <param name="bookingsPerSpecifiedDate"></param>
        private void CreateModalPopups(string aBookingTime, List<Booking> bookingsPerSpecifiedDate, ref HtmlGenericControl newModals)
        {
            DateTime convTime = Convert.ToDateTime(aBookingTime);
            
            // ############## Dynamiskt uppbyggd Bootstrapkod för vad som ska visas i Modal
            // ############## se "MODAL-KOD FÖR POPUP-FÖNSTRET" för kartläggning av alla controls
            HtmlGenericControl lvl01 = new HtmlGenericControl("div");
            lvl01.Attributes.Add("id", co.ToString());
            lvl01.Attributes.Add("class", "modal fade");
            lvl01.Attributes.Add("tabindex", "-1");
            lvl01.Attributes.Add("role", "dialog");
            lvl01.Attributes.Add("aria-labelledby", "myModalLabel");
            lvl01.Attributes.Add("aria-hidden", "true");
           

            HtmlGenericControl lvl02 = new HtmlGenericControl("div");
            lvl02.Attributes.Add("class", "modal-dialog");

            HtmlGenericControl lvl03 = new HtmlGenericControl("div");
            lvl03.Attributes.Add("class", "modal-content");

            HtmlGenericControl lvl04_header = new HtmlGenericControl("div");
            lvl04_header.Attributes.Add("class", "modal-header");
            HtmlGenericControl lvl04_headerText = new HtmlGenericControl("h4");
            lvl04_headerText.Attributes.Add("class", "modal-title");
            lvl04_headerText.Attributes.Add("id", "myModalLabel");
            lvl04_headerText.InnerHtml = "Bokningar " + convTime.ToShortTimeString();
            lvl04_header.Controls.Add(lvl04_headerText);

            HtmlGenericControl lvl04_body = new HtmlGenericControl("div");
            lvl04_body.Attributes.Add("class", "modal-body");

            int counter = 0;
            foreach (Booking item in bookingsPerSpecifiedDate)                                  // loopa genom de bokningar som finns för dagen
            {
                if (item.BookingTime.ToShortTimeString() == convTime.ToShortTimeString())       // reglerar att enbart aktuell tid (se aBookingTime) behandlas
                {
                    counter++;
                    HtmlGenericControl lvl04_bodyText = new HtmlGenericControl("p");
                    lvl04_bodyText.InnerHtml = "Bokningsdata här";
                    lvl04_body.Controls.Add(lvl04_bodyText);
                }
            }
            if (counter == 0)
            {
                // visa text att inget är bokat
                HtmlGenericControl lvl04_bodyText = new HtmlGenericControl("p");
                lvl04_bodyText.InnerHtml = "Inga bokningar";
                lvl04_body.Controls.Add(lvl04_bodyText);
            }

            HtmlGenericControl lvl04_footer = new HtmlGenericControl("div");
            lvl04_footer.Attributes.Add("class", "modal-footer");
            Button lvl04_footerButton01 = new Button();
            lvl04_footerButton01.Attributes.Add("class", "btn btn-default");
            lvl04_footerButton01.Attributes.Add("data-dismiss", "modal");
            lvl04_footerButton01.Text = "Stäng";

            Button lvl04_footerButton02 = new Button();
            lvl04_footerButton02.Attributes.Add("class", "btn btn-primary");
            lvl04_footerButton02.Text = "Bekräfta";
            if (counter == 0)
            {
                lvl04_footerButton02.Enabled = false;
            }
            lvl04_footer.Controls.Add(lvl04_footerButton01);
            lvl04_footer.Controls.Add(lvl04_footerButton02);

            // bygg ihop alla huvudtaggar av strukturen
            lvl03.Controls.Add(lvl04_header);
            lvl03.Controls.Add(lvl04_body);
            lvl03.Controls.Add(lvl04_footer);
            lvl02.Controls.Add(lvl03);
            lvl01.Controls.Add(lvl02);
            newModals.Controls.Add(lvl01);

            /* ######## HTML-KODEXEMPEL FÖR BOOTSTRAP MODAL ######## 
             * Nedan kod har testats att köras direkt i ett HTML-dokument. Används som mall för att bygga strukturen ovan.
             * 
             * <div class="span4 proj-div" data-toggle="modal" data-target="#GSCCModal"> asdfasd</div>
             *
             * ######## MODAL-KOD FÖR POPUP-FÖNSTRET ##########
             * lvl01 ---> <div id="GSCCModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
             * lvl02 --->     <div class="modal-dialog">
             * lvl03 --->          <div class="modal-content">
             * lvl04 --->              <div class="modal-header">
             *   x   --->                   <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;  </button>
             *   x   --->                   <h4 class="modal-title" id="myModalLabel">Modal title</h4>
             *   x   --->              </div>
             *   x   --->              <div class="modal-body">
             *   x   --->                   ...
             *   x   --->              </div>
             *   x   --->              <div class="modal-footer">
             *   x   --->                   <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
             *   x   --->                   <button type="button" class="btn btn-primary">Save changes</button>
             * lvl04 --->              </div>
             * lvl03 --->          </div>
             * lvl02 --->     </div>
             * lvl01 ---> </div>
             *  Källa: http://stackoverflow.com/questions/20111219/bootstrap-modals-how-to-open-with-onclick
             */
        }



        /// <summary>
        /// Lägger till visuell boknings-content till en HTML-div 
        /// för en viss tidpunkt i det aktuella bokningsschemat
        /// </summary>
        /// <param name="aBooking"></param>
        /// <param name="aBookingTime"></param>
        /// <param name="bookingsPerSpecifiedDate"></param>
        private void CreateButtonContent(ref HtmlGenericControl aBooking, string aBookingTime, List<Booking> bookingsPerSpecifiedDate)
        {
            co++;
            DateTime convTime = Convert.ToDateTime(aBookingTime);
            aBooking.Attributes.Add("id", "Booking "+ convTime.ToShortTimeString());
            // ############## Bootstrapkod för att trigga Modal (ej aBooking) 
            //aBooking.Attributes.Add("class", "aBooking span4 proj-div");
            string classData = "span4 proj-div";
            aBooking.Attributes.Add("data-toggle", "modal");
            aBooking.Attributes.Add("data-target", "#"+ co.ToString());

            int counter = 0;
            foreach (Booking item in bookingsPerSpecifiedDate)                                  // loopa genom de bokningar som finns för dagen
            {
                if (item.BookingTime.ToShortTimeString() == convTime.ToShortTimeString())       // reglerar att enbart aktuell tid (se aBookingTime) behandlas
                {
                    counter++;
                }
            }
            if (counter == 0)
            {
                classData += " aBooking";
            }
            else if (counter == 4)
            {
                classData += " aBookingFull";
            }
            else
            {
                classData += " aBookingSpotsLeft";
            }
            aBooking.Attributes.Add("class", classData);

            // ############## Info om vilket klockslag en viss bokning gäller
            HtmlGenericControl textInfo = new HtmlGenericControl("p");
            textInfo.InnerText = convTime.ToShortTimeString();
            aBooking.Controls.Add(textInfo);

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


        private void ChangeDay()
        {
            HtmlGenericControl ScheduelChangeDay = new HtmlGenericControl("div");
            ScheduelChangeDay.Attributes.Add("id", "changeDay");

            DisplayChangeDay.Controls.Add(ScheduelChangeDay);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            anyDate = DateTime.Now.AddDays(-1);
          
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            anyDate = DateTime.Now.AddDays(1);
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