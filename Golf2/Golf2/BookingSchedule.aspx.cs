using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

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
            ScorecardWithInfo.Visible = false;

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

            // genererar bokningsschemat, gör först en kontroll på om banan är öppen
            // Är den inte det, så genereras ett meddelande att den är stängd
            bool isCourseClosed = GenerateBookingSchedule();
             
            if (isCourseClosed)
            {
                GenerateCourseIsClosed();
                ScorecardWithInfo.Visible = false;
                dropdownscorecard.Visible = false;
                printScorecard.Visible = false;
            }

            ToolBox.checkIfUserIsAdmin(ref isadmin, Session["golfid"].ToString());


            if(isCourseClosed == false)
            {
                GenerateCheckinList(anyDate);
                
                if (isadmin == true)
                {
                        ScorecardWithInfo.Visible = true;
                        dropdownscorecard.Visible = true;
                        printScorecard.Visible = true;
                        dropdownSelectTee.Visible = true;
                        getScorecardinfo.Visible = true;
                    rubrikScorecard.Visible = true;
                    

                    if (!IsPostBack)
                    {
                        generateDropdownList();
                    }
                }
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
        private bool isadmin;
        private List<string> golfIdList;
        private Mail mail = new Mail();
        private List<string> memberList;
        #endregion


        /// <summary>
        /// Beroende på indata så tar metoden bort en eller samtliga spelare i en bokning.
        /// Därefter görs en kontroll om det finns några fler 
        /// </summary>
        /// <param name="removeAllPlayers"></param>
        /// <param name="golfIdToRemove"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [WebMethod]
        public static string deletePlayerFromBooking(string removeAllPlayers, string includedid, string bookingId, string time)
        {           
            string sql = "";    
            string result = "";
            Mail mail = new Mail();
            BindingList<string> GolfIds = new BindingList<string>();
            Page p = new Page();
            DateTime mailDate = Convert.ToDateTime(p.Session["NextDay"]);

            sql = "SELECT golfid FROM included WHERE bookingid = '" + bookingId.ToString() + "'";
            DataTable Table = new DataTable();
            ToolBox.SQL_NonParam(sql, ref Table);

            foreach (DataRow item in Table.Rows)
            {
                GolfIds.Add(item.ToString());
            }

            //skicka listan med golfids till send-metoden i Mail.aspx.cs
            mail.SendMail(mailDate, time, "Cancellation", GolfIds);
        
            if (Convert.ToBoolean(removeAllPlayers))
            {
                // deleta alla spelare i bokningen
                sql = "DELETE FROM included WHERE bookingid = '" + bookingId.ToString() + "'";
                ToolBox.SQL_NonParamCommand(sql, ref result);
            }
            else
            {            
                // deleta spelaren
                sql = "DELETE FROM included WHERE includedid = '" + includedid.ToString() + "'";
                ToolBox.SQL_NonParamCommand(sql, ref result);
            }

            

            IsIncludedTableEmptyForBooking(bookingId);      // rensar bort bokning om den är tom

            return null;
        }

        /// <summary>
        /// Kontrollerar för en viss bokning om includedtabellen 
        /// är tom (dvs. finns det folk bokade i bokningen?)
        /// Är den tom, så deletas bokningen i tabellen booking.
        /// </summary>
        private static void IsIncludedTableEmptyForBooking(string bookingId)
        {
            string result = "";
            string sql = "SELECT * FROM included WHERE bookingid = '" + bookingId.ToString() + "'";
            DataTable _table = new DataTable();
            // hämtar vilka som är bokade för en viss bokning
            ToolBox.SQL_NonParam(sql, ref _table);  

            // är inga spelare bokade, så deletas bokningen
            if (_table.Rows.Count == 0)
            {
                sql = "DELETE FROM booking WHERE bookingid = '" + bookingId.ToString() +"'";
                ToolBox.SQL_NonParamCommand(sql, ref result);
            }
        }

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
                table = new DataTable();
                string sql = "SELECT startclose, endclose FROM closed";

                ToolBox.SQL_NonParam(sql, ref table);


                HtmlGenericControl aBooking = new HtmlGenericControl("div");

                bool isBookingClosed = CheckIfTimeIsClosed(aBookingTime);

                if (isBookingClosed)
                {
                    CreateButtonContentClosed(aBookingTime, ref aBooking);
                }
                else
                {
                    CreateButtonContent(ref aBooking, aBookingTime, ShowBookings.BookingsPerSpecifiedDate); // skapar bokningsboxarna

                    CreateModalPopups(aBookingTime, ShowBookings.BookingsPerSpecifiedDate, ref newModals); // skapar modalspopuperna
                }




                Schedule.Controls.Add(aBooking);
            }

            HtmlGenericControl golfMembersList = new HtmlGenericControl("datalist");
            golfMembersList.Attributes.Add("id", "searchMembersList");
            foreach (string item in golfIdList)
            {
                HtmlGenericControl searchOptionsInList = new HtmlGenericControl("option");
                searchOptionsInList.InnerHtml = item.ToString();
                string tmp = "";

                foreach (char c in item.ToString())
                {
                    if (c == Convert.ToChar(" "))
                    {
                        searchOptionsInList.Attributes.Add("value", tmp);
                        tmp = "";
                        break;
                    }

                    tmp += c.ToString();
                }
                

                golfMembersList.Controls.Add(searchOptionsInList);
            }


            DisplayBookingSchedule.Controls.Add(Schedule);                          // den ihopbyggda HTML-strukturen läggs in i sidan
            DisplayBookingSchedule.Controls.Add(newModals);
            DisplayBookingSchedule.Controls.Add(golfMembersList);

            return isCourseClosed;                                                  // returnerar att banan är öppen
        }

        private void CreateButtonContentClosed(string aBookingTime, ref HtmlGenericControl aBooking)
        {
            DateTime convTime = Convert.ToDateTime(aBookingTime);
            aBooking.Attributes.Add("id", "Booking " + convTime.ToShortTimeString());

            aBooking.Attributes.Add("class", "aBookingIsClosed");

            // ############## Info om vilket klockslag en viss bokning gäller
            HtmlGenericControl textInfo = new HtmlGenericControl("p");
            textInfo.InnerText = convTime.ToShortTimeString();
            aBooking.Controls.Add(textInfo);

            HtmlGenericControl closedText = new HtmlGenericControl("p");
            closedText.InnerHtml = "STÄNGT !";
            closedText.Attributes.Add("class", "TimeIsClosed");
            aBooking.Controls.Add(closedText);


        }

        /// <summary>
        /// Kollar om en viss bokningstid är stängd
        /// </summary>
        /// <param name="aBookingTime"></param>
        /// <returns></returns>
        private bool CheckIfTimeIsClosed(string aBookingTime)                       
        {
            bool isItClosed = false;

            DateTime compare = Convert.ToDateTime(anyDate.ToShortDateString() + " " + aBookingTime);
            foreach (DataRow item in table.Rows)
            {
                bool startclose = (compare >= Convert.ToDateTime(item["startclose"])) ? true : false;

                bool endclose = (compare <= Convert.ToDateTime(item["endclose"])) ? true : false;

                if (startclose && endclose)
                {
                    isItClosed = true;
                    return isItClosed;
                }
            }

            return isItClosed;
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
            golfIdList = new List<string>();
            string sql = "SELECT golfid,firstname,surname FROM person;";
            table = new DataTable();
            ToolBox.SQL_NonParam(sql, ref table);

            foreach (DataRow item in table.Rows)
            {
                golfIdList.Add(item["golfid"].ToString() + " " + item["firstname"].ToString() + " " + item["surname"].ToString());                                      // lista skapas med alla existerande golfidn i databas
             //   golfIdList.Add(item["firstname"].ToString() + " " + item["surname"].ToString());
            }

            int counter = 0;    
            bool userIsAlreadyBookedThisTime = false;
            bool createdDivForAlreadyBookedUserCreated = false;
            foreach (Booking item in bookingsPerSpecifiedDate)                                  // loopa genom de bokningar som finns för dagen
            {

                if (item.BookingTime.ToShortTimeString() == convTime.ToShortTimeString())       // reglerar att enbart aktuell tid (se aBookingTime) behandlas
                {
                    counter++;                                                                  // räknar hur många som redan är bokade

                    HtmlGenericControl counterDiv = new HtmlGenericControl("div");
                    counterDiv.Attributes.Add("class", "bookingCounter");
                    HtmlGenericControl bookingCounter = new HtmlGenericControl("p");
                    bookingCounter.InnerHtml = counter.ToString();
                    counterDiv.Controls.Add(bookingCounter);

                    golfIdList.Remove(item.GolfId);                                             // golfidt rensas från listan, så att man inte kan dubbelboka en person samma tid
                    if (item.GolfId == Session["golfid"].ToString())
                    {
                        userIsAlreadyBookedThisTime = true;
                    }
                     

                    HtmlGenericControl lvl04_bodyTextDiv = new HtmlGenericControl("div");
                    HtmlGenericControl lvl04_bodyText = new HtmlGenericControl("p");
                    string bookingInfo = "";
                    // för css-formatering
                    if (userIsAlreadyBookedThisTime && !createdDivForAlreadyBookedUserCreated)
                    {
                        bookingInfo = "Du är inbokad på denna plats";
                        lvl04_bodyTextDiv.Attributes.Add("class", "aBookableSpot currMemberIsBooked");
                        createdDivForAlreadyBookedUserCreated = true;
                    }
                    else
                    {
                        bookingInfo += "Golf-id: " + item.GolfId + " - ";
                        bookingInfo += "Kön: " + item.Gender + " - ";
                        bookingInfo += "Hcp: " + item.Hcp.ToString();
                        maxTotalHcpLimit -= item.Hcp;                                               // spelarens hcp subtraheras från startbollens limit 
                        lvl04_bodyTextDiv.Attributes.Add("class", "aBookableSpot bookedSpot");             
                    }
                    lvl04_bodyText.InnerHtml = bookingInfo;



                    lvl04_body.Controls.Add(counterDiv);                // bokningsräknare tillagd

                    lvl04_bodyTextDiv.Controls.Add(lvl04_bodyText);
                    lvl04_body.Controls.Add(lvl04_bodyTextDiv);

                    HtmlGenericControl lvl04_bodyTextInputDiv = new HtmlGenericControl("div");
                    lvl04_bodyTextInputDiv.Attributes.Add("class", "aBookableSpotInput");
                    HtmlGenericControl lvl04_bodyTextReserveDiv = new HtmlGenericControl("div");
                    lvl04_bodyTextReserveDiv.Attributes.Add("class", "aBookableSpotReserve");

                    lvl04_body.Controls.Add(lvl04_bodyTextInputDiv);
                    lvl04_body.Controls.Add(lvl04_bodyTextReserveDiv);
                    
                    
                }
            }


            if (counter != 4)
            {
                HtmlGenericControl lvl04_autoReservation = new HtmlGenericControl("div");

                string golfid = Session["golfid"].ToString();

                bool isadmin = false;
                ToolBox.checkIfUserIsAdmin(ref isadmin, golfid);

                if (!isadmin && !userIsAlreadyBookedThisTime)
                {
                    counter++;
                    HtmlGenericControl counterDiv = new HtmlGenericControl("div");
                    counterDiv.Attributes.Add("class", "bookingCounter");
                    HtmlGenericControl bookingCounter = new HtmlGenericControl("p");
                    bookingCounter.InnerHtml = (counter).ToString();
                    counterDiv.Controls.Add(bookingCounter);

                    // Här reserveras den första platsen automatiskt på attribut reservation0 för bokaren
                    CreateAutomaticBookingForBooker(ref lvl04_autoReservation);
                    lvl04_body.Controls.Add(counterDiv);
                    lvl04_body.Controls.Add(lvl04_autoReservation);
                }

                for (int i = 0; i < 4-counter; i++)   // reservation1, reservation2 och reservation3 kan endast bokas för andra
                {
                    HtmlGenericControl counterDiv = new HtmlGenericControl("div");
                    counterDiv.Attributes.Add("class", "bookingCounter");
                    HtmlGenericControl bookingCounter = new HtmlGenericControl("p");
                    bookingCounter.InnerHtml = (counter + i + 1).ToString();


                    counterDiv.Controls.Add(bookingCounter);

                    // container för alla controls nedan
                    HtmlGenericControl lvl04_openBooking = new HtmlGenericControl("div");

                    // text som indikerar att en ledig plats finns att boka
                    HtmlGenericControl lvl04_bodyTextContainerDiv = new HtmlGenericControl("div");
                    lvl04_bodyTextContainerDiv.Attributes.Add("class", "aBookableSpot freeSpot");                    // för css-formatering
                    HtmlGenericControl lvl04_bodyText = new HtmlGenericControl("p");
                    lvl04_bodyText.InnerHtml = "Ledig plats";
                    lvl04_bodyText.Attributes.Add("id", convTime.ToShortTimeString() + i.ToString());
                    lvl04_bodyText.Attributes.Add("isReserved", "false");
                        

                    // container för gästbokningsknapp + och reservation/ångra-knapp 
                    HtmlGenericControl lvl04_bookingOptionsContainer = new HtmlGenericControl("div");
                    lvl04_bookingOptionsContainer.Attributes.Add("class", "bookingOptionsContainer");

                    // knapp för att reservera/ångra reservation
                    HtmlGenericControl lvl04_reserveFreeSpotButton = new HtmlGenericControl("input");
                    lvl04_reserveFreeSpotButton.Attributes.Add("id", convTime.ToShortTimeString() + "resereve" + i);
                    lvl04_reserveFreeSpotButton.Attributes.Add("class", "btn btn-primary reserveSpotButton");
                    lvl04_reserveFreeSpotButton.Attributes.Add("type", "button");
                    lvl04_reserveFreeSpotButton.Attributes.Add("value", "Reservera");
                    lvl04_reserveFreeSpotButton.Attributes.Add("class", "aBookableSpotReserve");       // för css-formatering
                    lvl04_reserveFreeSpotButton.Attributes.Add("onclick", "reservation(\'" + convTime.ToShortTimeString() + i.ToString() + "\', \'" + convTime.ToShortTimeString() + "searchMembers" + i + "\', \'" + "ContentPlaceHolder1_fakeSenderButton" + "\', \'" + i + "\', \'" + convTime.ToShortTimeString() + "resereve" + i + "\', \'" + isadmin + "\')");

                    // "snabbknapp" för gästbokning
                    HtmlGenericControl lvl04_bookAGuest = new HtmlGenericControl("input");
                    lvl04_bookAGuest.Attributes.Add("type", "button");
                    lvl04_bookAGuest.Attributes.Add("id", convTime.ToShortTimeString() + i.ToString()+"guestButton" + i.ToString());
                    lvl04_bookAGuest.Attributes.Add("value", "Boka en gäst");

                    // Förklaring på onclick: ="<id för vilken <p>-tag som skall ändras>, <fakebuttonknappens id>, <index för rad i modal>, <index för reserveringsknapp>, <bool för om nuvarande användare är admin> )
                    lvl04_bookAGuest.Attributes.Add("onclick", "reservationGuest(\'" + convTime.ToShortTimeString() + i.ToString() + "\', \'" + "ContentPlaceHolder1_fakeSenderButton" + "\', \'" + i + "\', \'" + convTime.ToShortTimeString() + "resereve" + i + "\', \'"+ isadmin +"\')");

                    // sökbart textfält för medlemmar
                    HtmlGenericControl searchGolfMember = new HtmlGenericControl("input");
                    searchGolfMember.Attributes.Add("id", convTime.ToShortTimeString() + "searchMembers" + i);
                    searchGolfMember.Attributes.Add("type", "text");
                    //searchGolfMember.Attributes.Add("list", convTime.ToShortTimeString() + "searchMembersList" + i);
                    searchGolfMember.Attributes.Add("list", "searchMembersList");
                    searchGolfMember.Attributes.Add("class", "aBookableSpotInput");                  // för css-formatering

                    // sökunderlaget för ovan sökbara textfält
                    //HtmlGenericControl golfMembersList = new HtmlGenericControl("datalist");
                    //golfMembersList.Attributes.Add("id", convTime.ToShortTimeString() + "searchMembersList" + i);
                    //foreach (string item in golfIdList)
                    //{
                    //    HtmlGenericControl searchOptionsInList = new HtmlGenericControl("option");
                    //    searchOptionsInList.Attributes.Add("value", item.ToString());
                    //    golfMembersList.Controls.Add(searchOptionsInList);
                    //}

                    lvl04_openBooking.Controls.Add(counterDiv);

                    lvl04_bodyTextContainerDiv.Controls.Add(lvl04_bodyText);
                    lvl04_openBooking.Controls.Add(lvl04_bodyTextContainerDiv);

                    lvl04_bookingOptionsContainer.Controls.Add(lvl04_bookAGuest);
                    lvl04_bookingOptionsContainer.Controls.Add(searchGolfMember);
                    //lvl04_bookingOptionsContainer.Controls.Add(golfMembersList);
                    lvl04_bookingOptionsContainer.Controls.Add(lvl04_reserveFreeSpotButton);

                    lvl04_openBooking.Controls.Add(lvl04_bookingOptionsContainer);
                    lvl04_body.Controls.Add(lvl04_openBooking);
                }
                
                
            }

            bool isAdmin = false;
            ToolBox.checkIfUserIsAdmin(ref isAdmin, Session["golfid"].ToString());
            
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
                lvl04_footerButton02.Attributes.Add("Value", "Bekräfta");
                lvl04_footerButton02.Attributes.Add("id", "Confirm" + convTime.ToShortTimeString());    // knappens id, för identifiering via javascript
                lvl04_footerButton02.Attributes.Add("currBookingTime", convTime.ToShortTimeString());   // bokningstid. Behövs för att genomföra bokning, datum finns sparat i anyDate-variabeln
                lvl04_footerButton02.Attributes.Add("type", "button");
                lvl04_footerButton02.Attributes.Add("onclick", "fulfix(\'"+ convTime.ToShortTimeString() + "\')");
                lvl04_footer.Controls.Add(lvl04_footerButton02);                                                 
            }



            // bygger ihop alla huvudtaggar av strukturen
            lvl03.Controls.Add(lvl04_header);
            lvl03.Controls.Add(lvl04_body);
            lvl03.Controls.Add(lvl04_footer);
            lvl02.Controls.Add(lvl03);
            lvl01.Controls.Add(lvl02);
            newModals.Controls.Add(lvl01);

            #region ######## HTML-KODEXEMPEL OCH STRUKTURMALL FÖR VÅR BOOTSTRAP MODAL ########
            /*  
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
            #endregion
        }

        /// <summary>
        /// skapar htmlstruktur för autoreservation av bokning för bokaren
        /// </summary>
        /// <param name="lvl04_autoReservation"></param>
        private void CreateAutomaticBookingForBooker(ref HtmlGenericControl lvl04_autoReservation)
        {
            lvl04_autoReservation.Attributes.Add("class", "aBookableSpot autoReservationLoggedInUser");
            fakeSenderButton.Attributes.Add("reservation0", Session["golfid"].ToString()); //automatisk bokning av den inloggade
            HtmlGenericControl autoReservation = new HtmlGenericControl("p");
            autoReservation.InnerHtml = "Plats reserverad för dig";

            lvl04_autoReservation.Controls.Add(autoReservation);
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

            bool isadmin = false;
            ToolBox.checkIfUserIsAdmin(ref isadmin, Session["golfid"].ToString());


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

                DateTime dt = Convert.ToDateTime(Session["NextDay"]);
                checkbooking = CheckBooking(item, dt);

                if (checkbooking == true)
                {
                    doublecheck = true;
                }
            }

            bool isUnique = IsGolfidUnique(cb, guestcount, isadmin);

            if (guestcount > 1 && isadmin == false)
            {
                moreguests = true;
                Session["error"] = "Bokningen är inte genomförd. Max 1 gäst per boll får anmälas";
            }


            if (isadmin == true) // om adminstatus finns, hoppa över filter
            {
                validdate = true;
                doublecheck = false;
                checkbooking = false;
            }

            if (validdate == false || moreguests == true || doublecheck == true || isUnique == false)
            {
                bookingAlertFail.Visible = true;
                bookingAlertsuccess.Visible = false;
                bookingAlertFail.InnerText = Session["error"].ToString();
            }
            else
            {
                int timeid = Convert.ToInt32(GetTimeID(timeHHMM));
                DateTime dt = Convert.ToDateTime(Session["NextDay"]);
                string owner = Convert.ToString(Session["golfid"]);
                int bookingid = 0;
                bool allclearforbooking = false;

                string sql = "SELECT bookingid " +
                            "FROM booking " +
                            "WHERE booking.bookingdate = @bookingdate AND booking.timeid = @timeid AND booking.owner = @owner";

                Postgress p = new Postgress();
                int exists = 0;
                exists = p.SQLCheckDateAndTime(sql, dt, timeid, owner);

                if (exists == 0) // om det inte redan finns tider bokade samma dag av samma owner
                {
                    sql = "INSERT INTO booking (bookingdate, courseid, timeid, owner) " +
                        "VALUES(@bookingdate, '1', @timeid, @owner) " +
                        "RETURNING bookingid AS integer";

                    bookingid = p.SQLBooking(sql, timeid, dt, owner);
                }

            else
                {
                    bookingid = exists;
                }

                foreach (string item in cb) //loopa igenom listan med golfids för att kontrollera om någon redan finns
                {

                    sql = "SELECT booking.bookingid " +
                           "FROM booking " +
                           "INNER JOIN included ON included.bookingid = booking.bookingid " +
                           "WHERE bookingdate = @bookingdate AND timeid = @timeid AND golfid = @golfid";

                    string includedexists = "";
                    includedexists = p.SQLCheckIncluded(sql, dt, timeid, item);

                    if (includedexists == "")
                    {
                        allclearforbooking = true;
                    }
                }

                if (allclearforbooking == true)
                {
                    foreach (string item in cb) //loopa igenom listan med golfids och lägg till i databasen
                    {
                        sql = "INSERT INTO included (bookingid, golfid) " +
                             "VALUES (@bookingid, @golfid)";

                        p.SQLbooking2(sql, item, bookingid);
                    }

                    mail = new Mail();
                    mail.SendMail(anyDate, timeHHMM + ":00", "booking", cb);
                    bookingAlertsuccess.Visible = true;
                    bookingAlertFail.Visible = false;
                    bookingAlertsuccess.InnerText = "Bokningen lyckades!";
                }
                else
                {
                    Session["error"] = "Bokningen är inte genomförd. Någon i bollen har redan en tid samma dag";              
                    bookingAlertFail.Visible = true;
                    bookingAlertsuccess.Visible = false;
                    bookingAlertFail.InnerText = Session["error"].ToString();
                }
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
        private bool CheckBooking(string golfid, DateTime date)
        {
            string sql = "SELECT booking.bookingid, booking.bookingdate " +
                        "FROM booking " +
                        "INNER JOIN included ON included.bookingid = booking.bookingid " +
                        "WHERE included.golfid = @golfid AND booking.bookingdate = @date";

            Postgress p = new Postgress();
            
            string exists = p.SQLCheckIfBooked(sql, golfid, date);

            //string searchbooking = Convert.ToString(Session["NextDay"]);
            //bool contains = false;
            //if (searchbooking != "")
            //{
            //    contains = table.AsEnumerable().Any(row => searchbooking == row.Field<String>(searchbooking));
            //}

            if (exists != "")
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
        private bool IsGolfidUnique(BindingList<string> cb, int guestcount, bool isadmin)
        {

            if (guestcount <= 1)
            {
                if (cb.Distinct().Count() == cb.Count())    //Jämför antalet unika golfid mot totalt antal golfid i listan
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
                int distinct = cb.Count() - cb.Distinct().Count();  // om antalet inte går ihop
                if (guestcount == distinct || isadmin == true)     
                {
                    return true;
                }

                Session["error"] = "Bokningen är inte genomförd. Antalet gäster stämmer inte";
                return false;
            }
        }


        /// <summary>
        /// Checkar in/ut en spelare
        /// </summary>
        /// <param name="includedid"></param>
        /// <param name="statusToChangeTo"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string TogglePlayerCheckin(string includedid, string statusToChangeTo)
        {
            string sql = "UPDATE included ";
            sql += "SET checkedin = "+ Convert.ToBoolean(statusToChangeTo) +" ";
            sql += "WHERE includedid = '" + Convert.ToInt32(includedid) + "'; ";

            string resultMessage = "";
            // ändra status för 
            ToolBox.SQL_NonParamCommand(sql, ref resultMessage);
            return resultMessage;
        }

        /// <summary>
        /// Genererar en incheckningslista
        /// </summary>
        /// <param name="date"></param>
        private void GenerateCheckinList(DateTime date)
        {
            string sql = "";

            if (isadmin)
            {
                // sqlsats för personal - söker alla bokningar för dagen
                sql = "SELECT person.golfid, person.firstname, person.surname, booking.bookingdate, bookingtime.time, included.bookingid, booking.owner, included.checkedin, included.includedid ";
                sql += "FROM included ";
                sql += "INNER JOIN person ON person.golfid = included.golfid ";
                sql += "INNER JOIN booking ON booking.bookingid = included.bookingid ";
                sql += "INNER JOIN bookingtime ON bookingtime.timeid = booking.timeid ";
                sql += "WHERE bookingdate = '" + anyDate + "' ";
                sql += "ORDER BY booking.timeid ASC;";
            }
            else
            {
                // sqlsats för medlem  - söker alla bokningar för dagen
                string currUser = Session["golfid"].ToString();
                sql = "SELECT person.golfid, person.firstname, person.surname, booking.bookingdate, bookingtime.time, included.bookingid, booking.owner, included.checkedin, included.includedid ";
                sql += "FROM included ";
                sql += "INNER JOIN person ON person.golfid = included.golfid ";
                sql += "INNER JOIN booking ON booking.bookingid = included.bookingid ";
                sql += "INNER JOIN bookingtime ON bookingtime.timeid = booking.timeid ";
                sql += "WHERE bookingdate = '" + anyDate + "' ";
                sql += "AND(owner = '" + currUser + "' OR person.golfid = '" + currUser + "') ";
                sql += "ORDER BY booking.timeid ASC; ";
            }


            table = new DataTable();
            ToolBox.SQL_NonParam(sql, ref table);

            HtmlGenericControl ListGroup = new HtmlGenericControl("div");
            ListGroup.Attributes.Add("id", "ListGroup");
            ListGroup.Attributes.Add("class", "list-group");



            foreach (DataRow item in table.Rows)
            {

                // Incheckning och avbokning får senast göras fem minuter innan golfrundan börjar
                // Här görs en kontroll på detta innan den bokade tiden får printas ut till webbsidan
                // Passering av tidsgränsen gör att bokningen ej listas
                //DateTime currentTime = DateTime.Now;
                //DateTime timeLimitation = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + item["time"]).AddMinutes(-5);
                bool skipThisLoop = ToolBox.timeLimitCheck(anyDate, item["time"].ToString());
                if (skipThisLoop)
                {
                    // ingenting görs  
                }
                else
                {
                    string concatenatedString = "";
                    //kontrollerar om det är ett gästkonto och filtrerar bort onödig info isf.
                    if (item["golfid"].ToString() == "Gäst")
                    {
                        concatenatedString = "Golfid: " + item["golfid"].ToString() + " ";
                    }
                    else
                    {
                        concatenatedString = "Golfid: " + item["golfid"].ToString() + ", ";
                        concatenatedString += item["FirstName"].ToString() + " " + item["SurName"].ToString() + " ";
                    }

                    // kontrollerar incheckningsstatus
                    HtmlGenericControl checkinDiv = new HtmlGenericControl("div");
                    checkinDiv.Attributes.Add("class", "checkInStatus");
                    checkinDiv.Attributes.Add("id", "chStatus" + item["includedid"].ToString());
                    if (Convert.ToBoolean(item["checkedin"]))
                    {
                        checkinDiv.InnerHtml = "Incheckad";
                        checkinDiv.Style.Add("background-color", "lightgreen");
                    }
                    else
                    {
                        checkinDiv.InnerHtml = "Ej incheckad";
                        checkinDiv.Style.Add("background-color", "lightgray");
                    }


                    HtmlGenericControl List = new HtmlGenericControl("div");

                    HtmlGenericControl teeTime = new HtmlGenericControl("div");
                    teeTime.Attributes.Add("class", "teeTimeInfo");
                    teeTime.InnerHtml = "Starttid: " + item["time"].ToString();

                    HtmlGenericControl bookedByInfo = new HtmlGenericControl("div");
                    bookedByInfo.Attributes.Add("class", "bookedByInfo");
                    bookedByInfo.InnerHtml = "bokad av " + item["owner"].ToString();

                    HtmlGenericControl listInfo = new HtmlGenericControl("div");
                    listInfo.InnerHtml = concatenatedString;
                    listInfo.Attributes.Add("class", "checkInInfo");

                    // Detta avsnitt styr vilken funktionalitet som blir tillgänglig beroende på roll
                    // Personal/admin: ser en incheckningsknapp + avbokningsknapp
                    // Medlem: ser bara avbokningsknapp
                    HtmlGenericControl checkInOutButton = new HtmlGenericControl("input");
                    if (isadmin)
                    {
                        checkInOutButton.Attributes.Add("class", "checkInButton");
                        if (!Convert.ToBoolean(item["checkedin"]))
                        {
                            checkInOutButton.Attributes.Add("value", "Checka in");
                            checkInOutButton.Attributes.Add("checkedin", "false");
                        }
                        else
                        {
                            checkInOutButton.Attributes.Add("value", "Ångra");
                            checkInOutButton.Attributes.Add("checkedin", "true");
                        }

                        checkInOutButton.Attributes.Add("golfid", item["golfid"].ToString());
                        checkInOutButton.Attributes.Add("type", "button");
                        checkInOutButton.Attributes.Add("id", "bookedTee" + item["includedid"].ToString());
                        //förklaring på onclick: <aktuell bokad spelares golfid>, <speltid>, <datum>, <PK i included-tabell>
                        checkInOutButton.Attributes.Add("onclick", "togglePlayerCheckin(\'" + item["golfid"].ToString() + "\', \'" + item["time"].ToString() + "\', \'" + item["bookingdate"].ToString() + "\', \'" + item["includedid"].ToString() + "\')");
                    }
                    // avbokningsknapp skapas
                    HtmlGenericControl cancelBookingButton = new HtmlGenericControl("input");
                    cancelBookingButton.Attributes.Add("value", "Avboka");
                    cancelBookingButton.Attributes.Add("id", "cancelTee" + item["includedid"].ToString());      // unikt id för knapp
                    cancelBookingButton.Attributes.Add("type", "button");
                    cancelBookingButton.Attributes.Add("class", "cancelTeeButton disable" + item["bookingid"].ToString());                             // css-formatering
                                                                                                                                                       //cancelBookingButton.Attributes.Add("owner", item["owner"].ToString());                      // vem som äger bokningen
                                                                                                                                                       //förklaring på onclick: <bokningsid> <includedid> <golfid att avbokas> <bokningsägare>
                    cancelBookingButton.Attributes.Add("onclick", "cancelPlayer(\'" + item["bookingid"].ToString() + "\', \'" + item["includedid"].ToString() + "\', \'" + item["golfid"].ToString() + "\', \'" + item["owner"].ToString() + "\',\'"+item["time"].ToString()+"\')");

                    List.Controls.Add(listInfo);
                    List.Controls.Add(teeTime);
                    List.Controls.Add(bookedByInfo);
                    List.Controls.Add(checkinDiv);
                    
                    // incheckningsknapp läggs till i struktur enbart om man är admin
                    if (isadmin)
                    {
                        List.Controls.Add(checkInOutButton);
                    }
                    List.Controls.Add(cancelBookingButton);
                    ListGroup.Controls.Add(List);
                }
            }


            // OBS! gamla koden som låg i BookingSchedule.aspx 170313
            //< div class="panel panel-default" id="panelIdTodaysBookings" runat="server" visible="true">
            //    <div class="panel-heading" id="TodaysBookings" runat="server" visible="true">Dagens Bokningar</div>
            //    <div class="panel-body" id="DisplayCheckIns" runat="server" visible="true"></div>
            //</div>

            HtmlGenericControl panelIdTodaysBookings = new HtmlGenericControl("div");
            panelIdTodaysBookings.Attributes.Add("class", "panel panel-default");

            HtmlGenericControl todaysBookings = new HtmlGenericControl("div");
            todaysBookings.Attributes.Add("class","panel-heading");
            todaysBookings.InnerHtml = "Dagens Bokningar";

            HtmlGenericControl DisplayCheckIns = new HtmlGenericControl("div");
            DisplayCheckIns.Attributes.Add("class", "panel-body");

            DisplayCheckIns.Controls.Add(ListGroup);

           
            panelIdTodaysBookings.Controls.Add(todaysBookings);
            panelIdTodaysBookings.Controls.Add(DisplayCheckIns);
            DisplayBookingSchedule.Controls.Add(panelIdTodaysBookings);

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
            
            generateDropdownList(); //Kod som krävs för att skapa dropdownlist
            clearScorecard();       //Kod som krävs för att rensa uppgifter i scorekortet
            // ####### 170308: ALL KOD FÖR ATT ÄNDRA DATUM HANTERAS ISTÄLLET PÅ CLIENT SIDE OCH FÅNGAS UPP I PAGE_LOAD
        }

        //OnClickEvents för att byta till nästkommande dag
        protected void Button2_Click(object sender, EventArgs e)
        {
            generateDropdownList(); //Kod som krävs för att skapa dropdownlist
            clearScorecard();       //Kod som krävs för att rensa uppgifter i scorekortet
            // ####### 170308: ALL KOD FÖR ATT ÄNDRA DATUM HANTERAS ISTÄLLET PÅ CLIENT SIDE OCH FÅNGAS UPP I PAGE_LOAD
        }






        #endregion

       
        //Skapar en dropdownlist med golfid baserat på dagens bokningar
        private void generateDropdownList()
        {
            DailyBookings bokning = new DailyBookings(anyDate);

            var name = from t in bokning.BookingsPerSpecifiedDate
                       
                       select new
                       {
                           
                           CompleteName = t.FirstName + " " + t.SurName + "#" + t.Hcp + "#" + t.BookingTime.ToShortTimeString() + "#" + t.Gender + "#",
                           GolfID = t.GolfId,

                       };

            dropdownscorecard.DataSource = name;
            dropdownscorecard.DataTextField = "GolfId";
            dropdownscorecard.DataValueField = "CompleteName";
            dropdownscorecard.DataBind();
        }

        //Metod för att rensa scorekortet
        private void clearScorecard()
        {
            scorecardDate.Text = "";
            scorecardGolfId.Text = "";
            scorecardHcp.Text = "";
            scorecardName.Text = "";
            scorecardSpelHcp.Text = "";
            scorecardTime.Text = "";
        }

        //Fyller scorekortet med aktuell information baserat på golfid och vald tee.
        protected void getScorecardinfo_Click(object sender, EventArgs e)
        {
            if(dropdownSelectTee.SelectedItem.Text != "Välj Tee")
            {
                

                string tmpscorecard = dropdownscorecard.SelectedItem.Value;
                List<string> person = new List<string>();
                string nyttOrd = "";

                foreach (char c in tmpscorecard)
                {
                    if (c == Convert.ToChar("#"))
                    {
                        person.Add(nyttOrd);
                        nyttOrd = "";
                    }
                    else
                    {
                        nyttOrd += c.ToString();
                    }
                }

                double spelHcp;
                double slope;
                double CR;
                double Par;
                double värde;

                if (person[3] == "Male")
                {
                    if (dropdownSelectTee.SelectedItem.Text == "Röd")
                    {
                        slope = 120;
                        CR = 67.8;
                    }
                    else
                    {
                        slope = 128;
                        CR = 71.4;
                    }
                    värde = 113;
                    Par = 72;
                    spelHcp = Convert.ToDouble(person[1]) * (slope / värde) + (CR - Par);

                }

                else
                {
                    if (dropdownSelectTee.SelectedItem.Text == "Röd")
                    {
                        slope = 124;
                        CR = 73;
                    }
                    else
                    {
                        slope = 133;
                        CR = 77.4;
                    }
                    värde = 113;
                    Par = 72;
                    spelHcp = Convert.ToDouble(person[1]) * (slope / värde) + (CR - Par);

                }

                var erhslag = Math.Round(spelHcp, 0, MidpointRounding.AwayFromZero);

                scorecardDate.Text = anyDate.ToShortDateString();
                scorecardGolfId.Text = dropdownSelectTee.SelectedItem.Text;
                scorecardName.Text = person[0];
                scorecardHcp.Text = person[1];
                scorecardTime.Text = person[2];
                scorecardSpelHcp.Text = erhslag.ToString();



                hole1Erh.Text = hole1Par.Text;
                hole2Erh.Text = hole2Par.Text;
                hole3Erh.Text = hole3Par.Text;
                hole4Erh.Text = hole4Par.Text;
                hole5Erh.Text = hole5Par.Text;
                hole6Erh.Text = hole6Par.Text;
                hole7Erh.Text = hole7Par.Text;
                hole8Erh.Text = hole8Par.Text;
                hole9Erh.Text = hole9Par.Text;
                hole10Erh.Text = hole10Par.Text;
                hole11Erh.Text = hole11Par.Text;
                hole12Erh.Text = hole12Par.Text;
                hole13Erh.Text = hole13Par.Text;
                hole14Erh.Text = hole14Par.Text;
                hole15Erh.Text = hole15Par.Text;
                hole16Erh.Text = hole16Par.Text;
                hole17Erh.Text = hole17Par.Text;
                hole18Erh.Text = hole18Par.Text;





                for (int i = 0; i < erhslag;)
                {




                    //index 1
                    if (i < erhslag)
                    {
                        int totalStrokes = Convert.ToInt32(hole15Erh.Text) + 1;
                        hole15Erh.Text = totalStrokes.ToString();
                        i++;
                    }
                    //Index 2
                    if (i < erhslag)
                    {
                        int totalStrokes = Convert.ToInt32(hole4Erh.Text) + 1;
                        hole4Erh.Text = totalStrokes.ToString();
                        i++;
                    }
                    //Index 3
                    if (i < erhslag)
                    {
                        int totalStrokes = Convert.ToInt32(hole13Erh.Text) + 1;
                        hole13Erh.Text = totalStrokes.ToString();
                        i++;
                    }
                    //Index 4
                    if (i < erhslag)
                    {
                        int totalStrokes = Convert.ToInt32(hole5Erh.Text) + 1;
                        hole5Erh.Text = totalStrokes.ToString();
                        i++;
                    }
                    //Index 5
                    if (i < erhslag)
                    {
                        int totalStrokes = Convert.ToInt32(hole18Erh.Text) + 1;
                        hole18Erh.Text = totalStrokes.ToString();
                        i++;
                    }
                    //Index 6
                    if (i < erhslag)
                    {
                        int totalStrokes = Convert.ToInt32(hole9Erh.Text) + 1;
                        hole9Erh.Text = totalStrokes.ToString();
                        i++;
                    }
                    //Index 7
                    if (i < erhslag)
                    {
                        int totalStrokes = Convert.ToInt32(hole12Erh.Text) + 1;
                        hole12Erh.Text = totalStrokes.ToString();
                        i++;
                    }
                    //Index 8
                    if (i < erhslag)
                    {
                        int totalStrokes = Convert.ToInt32(hole7Erh.Text) + 1;
                        hole7Erh.Text = totalStrokes.ToString();
                        i++;
                    }
                    //Index 9
                    if (i < erhslag)
                    {
                        int totalStrokes = Convert.ToInt32(hole10Erh.Text) + 1;
                        hole10Erh.Text = totalStrokes.ToString();
                        i++;
                    }
                    //Index 10
                    if (i < erhslag)
                    {
                        int totalStrokes = Convert.ToInt32(hole3Erh.Text) + 1;
                        hole3Erh.Text = totalStrokes.ToString();
                        i++;
                    }
                    //Index 11
                    if (i < erhslag)
                    {
                        int totalStrokes = Convert.ToInt32(hole17Erh.Text) + 1;
                        hole17Erh.Text = totalStrokes.ToString();
                        i++;
                    }
                    //Index 12
                    if (i < erhslag)
                    {
                        int totalStrokes = Convert.ToInt32(hole8Erh.Text) + 1;
                        hole8Erh.Text = totalStrokes.ToString();
                        i++;
                    }

                    //Index 13
                    if (i < erhslag)
                    {
                        int totalStrokes = Convert.ToInt32(hole11Erh.Text) + 1;
                        hole11Erh.Text = totalStrokes.ToString();
                        i++;
                    }
                    //Index 14
                    if (i < erhslag)
                    {
                        int totalStrokes = Convert.ToInt32(hole2Erh.Text) + 1;
                        hole2Erh.Text = totalStrokes.ToString();
                        i++;
                    }
                    //Index 15
                    if (i < erhslag)
                    {
                        int totalStrokes = Convert.ToInt32(hole16Erh.Text) + 1;
                        hole16Erh.Text = totalStrokes.ToString();
                        i++;
                    }
                    //Index 16
                    if (i < erhslag)
                    {
                        int totalStrokes = Convert.ToInt32(hole1Erh.Text) + 1;
                        hole1Erh.Text = totalStrokes.ToString();
                        i++;
                    }
                    //Index 17
                    if (i < erhslag)
                    {
                        int totalStrokes = Convert.ToInt32(hole14Erh.Text) + 1;
                        hole14Erh.Text = totalStrokes.ToString();
                        i++;
                    }
                    //Index 18
                    if (i < erhslag)
                    {
                        int totalStrokes = Convert.ToInt32(hole6Erh.Text) + 1;
                        hole6Erh.Text = totalStrokes.ToString();
                        i++;
                    }

                }


            }

            else
            {
                Response.Write("<script>alert('Välj vilken tee som scorekortet ska gälla.')</script>");
            }
        }

        protected void dropdownSelectTee_SelectedIndexChanged(object sender, EventArgs e)
        {
            getScorecardinfo.Enabled = true;
            printScorecard.Enabled = true;
        }
    }
}
