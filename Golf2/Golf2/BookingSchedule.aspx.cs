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
            if (HiddenChangeDateVariable.Value != "")
            {
                string clientSideChangeOfDate = HiddenChangeDateVariable.Value.ToString();
                Session["NextDay"] = clientSideChangeOfDate;
            }
            if (Session["NextDay"] == null)
            {
                Session["NextDay"] = DateTime.Now.ToShortDateString();
            }
            anyDate = Convert.ToDateTime(Session["NextDay"]);
            if (anyDate == DateTime.Today)
            {
                Button1.Visible = false;
            }
            else
            {
                Button1.Visible = true;
            }

            bool isCourseClosed = GenerateBookingSchedule();   // aspx validation postback, server control <-- läs på
            if (isCourseClosed)
            {
                GenerateCourseIsClosed();
            }



        }

        /// <summary>
        /// Genererar output på webbsidan att banan är stängd
        /// </summary>
        private void GenerateCourseIsClosed()
        {

            HtmlGenericControl containerDiv = new HtmlGenericControl("div");
            containerDiv.Attributes.Add("class", "closedCourse");
            HtmlGenericControl containerDiv2 = new HtmlGenericControl("div");
            containerDiv2.Attributes.Add("class", "centerTheText");
            HtmlGenericControl closedCourseParagraph = new HtmlGenericControl("p");
            closedCourseParagraph.Attributes.Add("class", "courseIsClosed");
            closedCourseParagraph.InnerHtml =  "Detta datum är banan stängd";

            containerDiv2.Controls.Add(closedCourseParagraph);
            containerDiv.Controls.Add(containerDiv2);
            DisplayBookingSchedule.Controls.Add(containerDiv);

        }

        #region ########## FIELDS ########## 

        private DataTable table;
        private DateTime anyDate;
        int co = 0;

        #endregion

        /// <summary>
        /// Bygger upp och printar ut bokningsschemat till sidan
        /// </summary>
        private bool GenerateBookingSchedule()
        {
            DailyBookings ShowBookings = new DailyBookings(anyDate);                // "dagens" bokningar + bokningsbara tider hämtas

            // Kontrollerar om listan med möjliga tider att boka är tom eller ej. 
            // Tom innebär att aktuella datumet ligger utanför banans start/slutdatum för öppna dagar
            bool isCourseClosed = (ShowBookings.AvailableBookingTimes.Count == 0) ? true : false;
            if (isCourseClosed)
            {
                return isCourseClosed;                                              // returnerar att banan är stängd
            }

            HtmlGenericControl Schedule = new HtmlGenericControl("div");            // behållare för schema-content skapas här
            Schedule.Attributes.Add("id", "schedule");

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

            return isCourseClosed;                                                  // returnerar att banan är öppen
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
                    lvl04_reserveFreeSpotButton.Attributes.Add("id", convTime.ToShortTimeString() + "resereve" + i);
                    lvl04_reserveFreeSpotButton.Attributes.Add("class", "btn btn-primary reserveSpotButton");
                    lvl04_reserveFreeSpotButton.Attributes.Add("type", "button");
                    lvl04_reserveFreeSpotButton.Attributes.Add("value", "Reservera");
                    lvl04_reserveFreeSpotButton.Attributes.Add("class", "aBookableSpot");       // för css-formatering
                    lvl04_reserveFreeSpotButton.Attributes.Add("onclick", "reservation(\'" + convTime.ToShortTimeString() + i.ToString() + "\', \'" + convTime.ToShortTimeString() + "searchMembers" + i + "\', \'" + "ContentPlaceHolder1_fakeSenderButton" + "\', \'" + i + "\', \'" + convTime.ToShortTimeString() + "resereve" + i + "\')");

                    // en sökbar lista skapas
                    HtmlGenericControl searchGolfMember = new HtmlGenericControl("input");
                    searchGolfMember.Attributes.Add("id", convTime.ToShortTimeString() + "searchMembers" + i);
                    searchGolfMember.Attributes.Add("type", "text");
                    searchGolfMember.Attributes.Add("list", convTime.ToShortTimeString() + "searchMembersList" + i);
                    searchGolfMember.Attributes.Add("class", "aBookableSpot");                  // för css-formatering

                    HtmlGenericControl golfMembersList = new HtmlGenericControl("datalist");
                    golfMembersList.Attributes.Add("id", convTime.ToShortTimeString() + "searchMembersList" + i);
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
            lvl04_footerButton01.Attributes.Add("onclick", "clearAllReservations(\'" + convTime.ToShortTimeString() + "\', \'" + convTime.ToShortTimeString() + "resereve" + "\', \'" + "ContentPlaceHolder1_fakeSenderButton" + "\')");  

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

                HtmlGenericControl lvl04_footerButton02 = new HtmlGenericControl("input");
                lvl04_footerButton02.Attributes.Add("class", "btn btn-primary");
                //lvl04_footerButton02.InnerHtml = "Bekräfta";
                lvl04_footerButton02.Attributes.Add("Value", "Bekräfta");
                lvl04_footerButton02.Attributes.Add("id", "Confirm" + convTime.ToShortTimeString());    // knappens id, för identifiering via javascript
                lvl04_footerButton02.Attributes.Add("currBookingTime", convTime.ToShortTimeString());   // bokningstid. Behövs för att genomföra bokning, datum finns sparat i anyDate-variabeln
                lvl04_footerButton02.Attributes.Add("type", "button");
                lvl04_footerButton02.Attributes.Add("onclick", "fulfix(\'"+ convTime.ToShortTimeString() + "\')");
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
        /// Metod för att kontrollera och lägga in spelare i databasen, med filter (för admin gäller ej filter)
        /// </summary>
        /// <param name="cb"></param>
        private void ConfirmBooking(BindingList<string> cb)
        {

            string timeHHMM = cb.Last();    //spara och ta bort sista elementet (time) ur listan från "Bekräfta"-knappen
            cb.RemoveAt(cb.Count - 1);


            Postgress p = new Postgress();
            bool isadmin = true;
            if (Convert.ToString(Session["golfid"]).Contains("_"))
            {
                isadmin = false;
            }
            
            bool validdate = BookingValidDate();
            bool checkbooking = false;
            bool doublecheck = false;
            bool moreguests = false;
            int guestcount = 0;

            foreach (string item in cb) //Loopa igenom listan med golfids och sätt true om personen finns, 
            {                           //doublecheck för att värdet inte ska skrivas över i loopen
                if (item == "Gäst")     //kontrollerar antal gäster
                {
                    guestcount++;
                }
                checkbooking = CheckBooking(item);

                if (checkbooking == true)
                {
                    doublecheck = true;
                }
            }

            bool isUnique = IsGolfidUnique(cb, guestcount);
            
            if (guestcount > 1 && isadmin == false) 
            {
                moreguests = true;
                Session["error"] = "Bokningen är inte genomförd. Max 1 gäst per boll får anmälas";
            }


            if (isadmin == true) // om adminstatus finns, hoppa över filter
            {
                validdate = true;
                doublecheck = false;
            }

            if (validdate == false || moreguests == true || doublecheck == true || isUnique == false)
            {
                Response.Write("<script>alert('" + Convert.ToString(Session["error"]) + "')</script>");
            }
            else
            {
                int timeid = Convert.ToInt32(GetTimeID(timeHHMM));
                DateTime dt = Convert.ToDateTime(Session["NextDay"]);
                string owner = Convert.ToString(Session["golfid"]);
                int bookingid = 0;

                string sql ="SELECT bookingid " +
                            "FROM booking " +
                            "WHERE booking.bookingdate = @bookingdate AND booking.timeid = @timeid";

                int exists = 0;
                exists = p.SQLCheckDateAndTime(sql, dt, timeid);

                if (exists == 0) // om det inte redan finns bokade på samma tid samma dag
                {
                    sql = "INSERT INTO booking (bookingdate, courseid, timeid, owner) " +
                        "VALUES(@bookingdate, '1', @timeid, @owner) " +
                        "RETURNING bookingid AS integer";

                    bookingid = p.SQLBooking(sql, timeid, dt, owner);
                }

                foreach (string item in cb) //loopa igenom listan med golfids och lägg till givet bookingid
                {
                    sql ="INSERT INTO included (bookingid, golfid) " +
                         "VALUES (@bookingid, @golfid)";

                    p.SQLbooking2(sql, item, bookingid);
                }

                Response.Write("<script>alert('Bokningen lyckades. Golf's up')</script>");
            }
        }

        /// <summary>
        /// Kontroll om bokningen är inom en månad
        /// </summary>
        /// <returns>true/false</returns>
        private bool BookingValidDate()
        {
            DateTime bookingdate = Convert.ToDateTime(Session["NextDay"]);

            DateTime bookingexpiry = bookingdate.AddDays(30);

            if (DateTime.Today > bookingexpiry.Date)
            {
                Session["error"] = "Bokningen är inte genomförd. Tider får bokas max 30 dagar framåt";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Kontrollerar om någon i sällskapet har en bokad tid samma dag
        /// </summary>
        /// <param name="golfid"></param>
        /// <returns></returns>
        private bool CheckBooking(string golfid)
        {
            string sql = "SELECT booking.bookingid, booking.bookingdate " +
                        "FROM booking " +
                        "INNER JOIN included ON included.bookingid = booking.bookingid " +
                        "WHERE included.golfid =" + golfid;

            table = new DataTable();
            ToolBox.SQL_NonParam(sql, ref table);

            string searchbooking = Convert.ToString(Session["NextDay"]);
            bool contains = false;

            if (searchbooking != "")
            {
                contains = table.AsEnumerable().Any(row => searchbooking == row.Field<String>(searchbooking));
            }


            if (contains == true)
            {
                Session["error"] = "Bokningen är inte genomförd. En person i bollen har redan en bokad tid samma dag!";
                return true;
            }
            else { return false; }
        }

        /// <summary>
        /// Hämtar timeid från databsen utifrån time format (HH:MM:SS) i DateTime för vald bokningsdag 
        /// </summary>
        /// <param name="bookingtime"></param>
        /// <returns></returns>
        private int GetTimeID(string bookingtime)
        {
            TimeSpan bt = TimeSpan.Parse(bookingtime += ":00");

            string sql = "SELECT timeid FROM bookingtime WHERE time = '" + bt + "'";

            DataTable table = new DataTable();
            ToolBox.SQL_NonParam(sql, ref table);
            int timeid = 0;

            if (table.Rows[0] != null)
            {
                timeid = Convert.ToInt32(table.Rows[0]["timeid"]);
            }
            
            return timeid;
        }

        /// <summary>
        /// Kontrollerar att inte samma golfid har angivits flera gånger i samma boll vid bokning
        /// </summary>
        /// <param name="cb"></param>
        /// <returns></returns>
        private bool IsGolfidUnique(BindingList<string> cb, int guestcount)
        {

            if (guestcount <= 1)
            {
                if (cb.Distinct().Count() == cb.Count())
                {

                    return true;
                }
                else
                {
                    Session["error"] = "Bokningen är inte genomförd. Golfid kan endast anmälas en gång per dag";
                    return false;
                }

            }
            else
            {
                int distinct = cb.Count() - cb.Distinct().Count();
                if (guestcount == distinct)
                {
                    return true;
                }
                Session["error"] = "Bokningen är inte genomförd. Antalet gäster stämmer inte";
                return false;
            }
        }
        


        #region ############ EVENT HANDLERS HÄR ############ 
        /// <summary>
        /// Eventhandler som hämtar bokningsdata
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fakeSenderButton_Command(object sender, CommandEventArgs e)
        {
            string bookingContent = hdnfldVariable.Value;
            BindingList<string> booking = new BindingList<string>();
            string tmp = "";
            foreach (char c in bookingContent)
            {
                if (c == Convert.ToChar("#"))
                {
                    booking.Add(tmp);
                    tmp = "";
                }
                else
                {
                    tmp += c.ToString();
                }
            }
            booking.Add(tmp);

            ConfirmBooking(booking);

            /* Dokumentation för användning:
             * booking-listvariabeln kan skickas vidare till den metod som hanterar bokningen.
             * Denna lista kan max innehålla 4 golfidn + 1 bokningstid
             * datumet finns sparat i sessionsvariabeln session["NextDay"]
            */
        }



        protected void Button1_Click(object sender, EventArgs e)
        {

            // ####### 170308: ALL KOD FÖR ATT ÄNDRA DATUM HANTERAS ISTÄLLET PÅ CLIENT SIDE OCH FÅNGAS UPP I PAGE_LOAD



            //anyDate = Convert.ToDateTime(Session["NextDay"]);
            //if (anyDate != DateTime.Today)
            //{
            //    anyDate = anyDate.AddDays(-1);
            //    Session["NextDay"] = anyDate.ToShortDateString();
            //}
            //GenerateBookingSchedule();



            //if (anyDate.ToShortTimeString() == DateTime.Now.ToShortTimeString())
            //{
            //    Button1.Visible = false;
            //    GenerateBookingSchedule();
            //}

            //else if (anyDate > DateTime.Now)
            //{
            //    anyDate = anyDate.AddDays(-1);
            //    Session["NextDay"] = anyDate.ToString();
            //    anyDate = Convert.ToDateTime(Session["NextDay"]);
            //    GenerateBookingSchedule();

            //}
        }

        //OnClickEvents för att byta till nästkommande dag
        protected void Button2_Click(object sender, EventArgs e)
        {
            
            
            // ####### 170308: ALL KOD FÖR ATT ÄNDRA DATUM HANTERAS ISTÄLLET PÅ CLIENT SIDE OCH FÅNGAS UPP I PAGE_LOAD




            //anyDate = Convert.ToDateTime(Session["NextDay"]);
            //anyDate = anyDate.AddDays(+1);
            //Session["NextDay"] = anyDate.ToShortDateString();
            //GenerateBookingSchedule();
            //string clientSideChangeOfDate = HiddenChangeDateVariable.Value.ToString();

            //Session["NextDay"] = clientSideChangeOfDate;


            //anyDate = anyDate.AddDays(+1);
            //if (IsPostBack)
            //{
            //    // anyDate = anyDate.AddDays(+1);

            //    Session["NextDay"] = anyDate.ToString();
            //    anyDate = Convert.ToDateTime(Session["NextDay"]);

            //    GenerateBookingSchedule();
            //    Button1.Visible = true;
            //}
        }





        #endregion


    }
}
