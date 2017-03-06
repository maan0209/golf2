using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI.HtmlControls;
using System.Data;

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
            Button1.Visible = false;
            if (!IsPostBack)
            {
                GenerateBookingSchedule();

            }


        }

        #region ########## FIELDS ########## 

        private DataTable table;
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
        /// Skapar modals popuper och dess innehåll.
        /// Visar mer detaljerad status för bokningstiden om hur
        /// många som bokat sig och om det finns lediga platser.
        /// </summary>
        /// <param name="aBookingTime"></param>
        /// <param name="bookingsPerSpecifiedDate"></param>
        private void CreateModalPopups(string aBookingTime, List<Booking> bookingsPerSpecifiedDate, ref HtmlGenericControl newModals)
        {
            DateTime convTime = Convert.ToDateTime(aBookingTime);
            double maxTotalHcpLimit = 120;

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

            // läs ut lista med golfidn
            List<string> golfIdList = new List<string>();
            string sql = "SELECT golfid FROM person;";
            table = new DataTable();
            ToolBox.SQL_NonParam(sql, ref table);

            foreach (DataRow item in table.Rows)
            {
                golfIdList.Add(item["golfid"].ToString());                                      // lista skapas med alla existerande golfidn i databas
            }

            int counter = 0;
            foreach (Booking item in bookingsPerSpecifiedDate)                                  // loopa genom de bokningar som finns för dagen
            {

                if (item.BookingTime.ToShortTimeString() == convTime.ToShortTimeString())       // reglerar att enbart aktuell tid (se aBookingTime) behandlas
                {
                    counter++;
                    HtmlGenericControl lvl04_bodyText = new HtmlGenericControl("p");
                    string bookingInfo = "";
                    bookingInfo += "Golf-id: " + item.GolfId + " - ";
                    bookingInfo += "Kön: " + item.Gender + " - ";
                    bookingInfo += "Hcp: " + item.Hcp.ToString();
                    maxTotalHcpLimit -= item.Hcp;                                               // spelarens hcp subtraheras från startbollens limit 
                    lvl04_bodyText.InnerHtml = bookingInfo;

                    lvl04_body.Controls.Add(lvl04_bodyText);
                    golfIdList.Remove(item.GolfId);                                             // golfidt rensas från listan, så att man inte kan dubbelboka en person samma tid
                }
            }


            if (counter != 4)
            {

                for (int i = 0; i < 4 - counter; i++)
                {
                    HtmlGenericControl lvl04_openBooking = new HtmlGenericControl("div");
                    HtmlGenericControl lvl04_bodyText = new HtmlGenericControl("p");
                    lvl04_bodyText.InnerHtml = "Ledig plats";
                    lvl04_bodyText.Attributes.Add("class", "aBookableSpot");                    // för css-formatering
                    lvl04_bodyText.Attributes.Add("id", convTime.ToShortTimeString() + i.ToString());
                    lvl04_bodyText.Attributes.Add("isReserved", "false");
                    lvl04_openBooking.Controls.Add(lvl04_bodyText);

                    HtmlGenericControl lvl04_reserveFreeSpotButton = new HtmlGenericControl("input");
                    lvl04_reserveFreeSpotButton.Attributes.Add("class", "btn btn-primary reserveSpotButton");
                    lvl04_reserveFreeSpotButton.Attributes.Add("type", "button");
                    lvl04_reserveFreeSpotButton.Attributes.Add("value", "Reservera");
                    //lvl04_reserveFreeSpotButton.Attributes.Add("reservation", convTime.ToShortTimeString() + i.ToString());
                    lvl04_reserveFreeSpotButton.Attributes.Add("class", "aBookableSpot");       // för css-formatering
                    lvl04_reserveFreeSpotButton.Attributes.Add("onclick", "reservation(\'" + convTime.ToShortTimeString() + i.ToString() + "\')");

                    // en sökbar lista skapas
                    HtmlGenericControl searchGolfMember = new HtmlGenericControl("input");
                    searchGolfMember.Attributes.Add("list", "ContentPlaceHolder1_" + convTime.ToShortTimeString() + "searchMembers");
                    searchGolfMember.Attributes.Add("class", "aBookableSpot");                  // för css-formatering
                    HtmlGenericControl golfMembersList = new HtmlGenericControl("datalist");
                    golfMembersList.ID = convTime.ToShortTimeString() + "searchMembers";
                    foreach (string item in golfIdList)
                    {
                        HtmlGenericControl searchOptionsInList = new HtmlGenericControl("option");
                        searchOptionsInList.Attributes.Add("value", item.ToString());
                        golfMembersList.Controls.Add(searchOptionsInList);
                    }

                    lvl04_openBooking.Controls.Add(searchGolfMember);
                    lvl04_openBooking.Controls.Add(golfMembersList);
                    lvl04_openBooking.Controls.Add(lvl04_reserveFreeSpotButton);
                    lvl04_body.Controls.Add(lvl04_openBooking);
                }
            }


            HtmlGenericControl lvl04_footer = new HtmlGenericControl("div");
            lvl04_footer.Attributes.Add("class", "modal-footer");
            Button lvl04_footerButton01 = new Button();
            lvl04_footerButton01.Attributes.Add("class", "btn btn-primary");
            lvl04_footerButton01.Attributes.Add("data-dismiss", "modal");
            lvl04_footerButton01.Text = "Stäng";

            // generera en tag som visar max tillåten hcp
            HtmlGenericControl maxHcpAllowed = new HtmlGenericControl("p");
            maxHcpAllowed.Attributes.Add("id", convTime.ToShortTimeString() + "maxhcp");
            maxHcpAllowed.Attributes.Add("hcp", maxTotalHcpLimit.ToString());
            double textHcpOutput = 0;
            if (counter != 4)
            {
                if (maxTotalHcpLimit > 36)
                {
                    textHcpOutput = 36;
                }
                else
                {
                    textHcpOutput = maxTotalHcpLimit;
                }
                maxHcpAllowed.InnerHtml = "Max tillåten hcp: " + textHcpOutput.ToString();
            }
            lvl04_footer.Controls.Add(maxHcpAllowed);

            lvl04_footer.Controls.Add(lvl04_footerButton01);
            if (counter != 4)
            {
                Button lvl04_footerButton02 = new Button();
                lvl04_footerButton02.Attributes.Add("class", "btn btn-primary");
                lvl04_footerButton02.Text = "Bekräfta";
                lvl04_footerButton02.Attributes.Add("id", "Confirm" + convTime.ToShortTimeString());    // knappens id, för identifiering via javascript
                lvl04_footerButton02.Attributes.Add("reservation0", "");                                // lagringsplats för reservationer
                lvl04_footerButton02.Attributes.Add("reservation1", "");
                lvl04_footerButton02.Attributes.Add("reservation2", "");
                lvl04_footerButton02.Attributes.Add("reservation3", "");
                lvl04_footerButton02.Attributes.Add("currBookingTime", convTime.ToShortTimeString());   // bokningstid. Behövs för att genomföra bokning, datum finns sparat i anyDate-variabeln

                lvl04_footer.Controls.Add(lvl04_footerButton02);
            }



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
        /// Skapar bokningsschemat som visar vilka tider som är
        /// öppna att boka samt vilka som är fullbokade eller lediga.
        /// </summary>
        /// <param name="aBooking"></param>
        /// <param name="aBookingTime"></param>
        /// <param name="bookingsPerSpecifiedDate"></param>
        private void CreateButtonContent(ref HtmlGenericControl aBooking, string aBookingTime, List<Booking> bookingsPerSpecifiedDate)
        {
            co++;
            DateTime convTime = Convert.ToDateTime(aBookingTime);
            aBooking.Attributes.Add("id", "Booking " + convTime.ToShortTimeString());
            // ############## Bootstrapkod för att trigga Modal (ej aBooking) 
            string classData = "span4 proj-div";
            aBooking.Attributes.Add("data-toggle", "modal");
            aBooking.Attributes.Add("data-target", "#" + co.ToString());

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

            HtmlGenericControl aRow = new HtmlGenericControl("div");
            aRow.Attributes.Add("class", "badgeRow");
            HtmlGenericControl amountBooked = new HtmlGenericControl("span");
            amountBooked.Attributes.Add("class", "badge");
            amountBooked.InnerHtml = counter.ToString() + "/4";
            aRow.Controls.Add(amountBooked);
            aBooking.Controls.Add(aRow);

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
        private void MemberFilters(DateTime bookingdate, string golfid, int totalguests, double hcp, double hcp2, double hcp3, double hcp4)
        {

            bool bookingvalid = BookingValidDate(bookingdate);
            bool bookinghcp = BookingHcp(hcp, hcp2, hcp3, hcp4);
            bool bookingguests = Guests(totalguests);
            bool checkbooking = CheckBooking(golfid, bookingdate);

            if (bookingvalid || bookinghcp || bookingguests || checkbooking == true)
            {
                //ingen bokning
            }
            else
            {
                int timeid = GetTimeID(bookingdate);

                string sql = "INSERT INTO booking (bookingdate, courseid, timeid) " +
                             "VALUES(@bookingdate, '1', @timeid) " +
                             "RETURNING bookingid AS integer";

                Postgress p = new Postgress();
                int bookingid = p.SQLBooking(sql, timeid, bookingdate);

                sql = "INSERT INTO included";




            }
        }

        /// <summary>
        /// Kontroll om bokningen är inom en månad
        /// </summary>
        /// <param name="bookingdate"></param>
        /// <returns>true/false</returns>
        private bool BookingValidDate(DateTime bookingdate)
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
        private bool BookingHcp(double hcp, double hcp2, double hcp3, double hcp4)
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


        //OnClickEvents för att byta till föregående dag
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (anyDate.ToShortTimeString() == DateTime.Now.ToShortTimeString())
            {
                Button1.Visible = false;
                GenerateBookingSchedule();
            }

            else if (anyDate > DateTime.Now)
            {
                anyDate = anyDate.AddDays(-1);
                Session["LastDay"] = anyDate.ToString();
                anyDate = Convert.ToDateTime(Session["LastDay"]);
                GenerateBookingSchedule();

            }
        }

        //OnClickEvents för att byta till nästkommande dag
        protected void Button2_Click(object sender, EventArgs e)
        {
            anyDate = anyDate.AddDays(+1);
            if (IsPostBack)
            {
                // anyDate = anyDate.AddDays(+1);

                Session["NextDay"] = anyDate.ToString();
                anyDate = Convert.ToDateTime(Session["NextDay"]);

                GenerateBookingSchedule();
                Button1.Visible = true;
            }
        }



        /// <summary>
        /// Hämtar namet utifrån angivet golfid i bokning
        /// </summary>
        /// <param name="golfid"></param>
        /// <returns></returns>
        private string GetNameBooking(string golfid)
        {

            string sql = "SELECT firstname, surname, hcp FROM person WHERE golfid =" + golfid;

            table = new DataTable();
            ToolBox.SQL_NonParam(sql, ref table);
            Person person = new Person();

            foreach (DataRow row in table.Rows)
            {

                person.FirstName = (string)row["firstname"];
                person.SurName = (string)row["surname"];
                person.Hcp = Math.Round(Convert.ToDouble(row["hcp"]), 1);

            }
            return person.FirstName + " " + person.SurName + ": HCP: " + person.Hcp;
        }

        /// <summary>
        /// Kontrollerar om någon i sällskapet har en bokadtid samma dag
        /// </summary>
        /// <param name="golfid"></param>
        /// <param name="bookingdate"></param>
        /// <returns></returns>
        private bool CheckBooking(string golfid, DateTime bookingdate)
        {
            string sql = "SELECT SELECT booking.bookingid, booking.bookingdate " +
                        "FROM booking " +
                        "INNER JOIN included ON included.bookingid = booking.bookingid " +
                        "WHERE included.golfid =" + golfid;

            table = new DataTable();
            ToolBox.SQL_NonParam(sql, ref table);
            Person person = new Person();

            string searchbooking = bookingdate.ToShortDateString();
            bool contains = table.AsEnumerable().Any(row => searchbooking == row.Field<String>(searchbooking));

            if (contains == true)
            {
                Session["error"] = "Denna person har redan en bokad tid idag!";
                return true;
            }
            else { return false; }

        }

        /// <summary>
        /// Hämtar timeid från databsen utifrån time format (HH:MM:SS) i DateTime för vald bokningsdag 
        /// </summary>
        /// <param name="bookingdate"></param>
        /// <returns></returns>
        private int GetTimeID(DateTime bookingdate)
        {

            string timestring = bookingdate.ToLongTimeString();

            string sql = "SELECT timeid FROM bookingtime WHERE time =" + timestring;

            table = new DataTable();
            ToolBox.SQL_NonParam(sql, ref table);

            int timeid = (int)table.Rows[0]["timeid"];

            return timeid;

        }


    }
}
