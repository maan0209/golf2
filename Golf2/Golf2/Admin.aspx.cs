using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Golf2
{
    public partial class Admin : System.Web.UI.Page
    {
        Postgress db = new Postgress();

        List<course> seasondates = new List<course>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Postgress p = new Postgress();
                string sql = "select time from bookingtime ORDER BY time ASC";



                openhour.DataSource = p.sqlquestion(sql);
                openhour.DataTextField = "time";
                openhour.DataBind();

                Postgress p2 = new Postgress();
                closehour.DataSource = p2.sqlquestion(sql);
                closehour.DataTextField = "time";
                closehour.DataBind();


                //STÄNGA DAGAR TID
                Postgress p3 = new Postgress();

                dropfirstclose.DataSource = p3.sqlquestion(sql);
                dropfirstclose.DataTextField = "time";
                dropfirstclose.DataBind();

                Postgress p4 = new Postgress();

                droplastclose.DataSource = p4.sqlquestion(sql);
                droplastclose.DataTextField = "time";
                droplastclose.DataBind();

                droplastclose.SelectedIndex = droplastclose.Items.Count - 1;

                Calendar3.SelectedDate = DateTime.Today;
                Calendar4.SelectedDate = DateTime.Today;
                Calendar3.VisibleDate = DateTime.Today;
                Calendar4.VisibleDate = DateTime.Today;


                // VISA DATUM SÄSONG
                sql = "SELECT startdate, enddate FROM course";
                DataTable Table = new DataTable();
                ToolBox.SQL_NonParam(sql, ref Table);

                DateTime start = Convert.ToDateTime(Table.Rows[0]["startdate"]);
                DateTime end = Convert.ToDateTime(Table.Rows[0]["enddate"]);

                lblStartdatum.Text = start.ToShortDateString();
                lblSlutdatum.Text = end.ToShortDateString();

                Calendar1.SelectedDate = start;
                Calendar1.VisibleDate = start;
                Calendar2.SelectedDate = end;
                Calendar2.VisibleDate = end;

                updateclosinglist();
            }
        


        }

        private void updateclosinglist()
        {
            listcloseddays.Items.Clear();
            // VISA STÄNGDA DATUM
            Postgress p5 = new Postgress();
            String sql = "SELECT * FROM closed";


            DataTable roligtnamn = new DataTable();
            ToolBox.SQL_NonParam(sql, ref roligtnamn);

            foreach (DataRow Item in roligtnamn.Rows)
            {
                listcloseddays.Items.Add(Item["closedid"].ToString() + ": Banan stänger: " + Item["startclose"].ToString() + " Banan öppnar: " + Item["endclose"].ToString());
            }
        }

        protected void setOpenClose_Click(object sender, EventArgs e)
        {
            if (openhour.SelectedIndex > closehour.SelectedIndex)
            {
                Response.Write("<script>alert('Kontrollera tiderna, stängning kan ej ske innan öppning.')</script>");
            }
            else

            {
                Postgress p = new Postgress();
                string öppnar = openhour.SelectedValue.ToString();
                string stänger = closehour.SelectedValue.ToString();

                string sql = "UPDATE bookingtime set active = 'f' WHERE time >= '00:00:00' AND time <= '23:50:00'";
                p.sqlquestion(sql);

                Postgress uppdatera = new Postgress();
                string sqluppdatera = "UPDATE bookingtime set active = 't' WHERE time >= '" + öppnar + "' AND time <= '" + stänger + "'";
                uppdatera.sqlquestion(sqluppdatera);

                Response.Write("<script>alert('Öppettiderna är justerade.')</script>");
            }
        }

        protected void saveSeason_Click(object sender, EventArgs e)
        {
            string Startdate = Calendar1.SelectedDate.ToString("yyyy-MM-dd");
            string Enddate = Calendar2.SelectedDate.ToString("yyyy-MM-dd");

            DateTime newstartdate = Convert.ToDateTime(Startdate);
            DateTime newenddate = Convert.ToDateTime(Enddate);
                    
            db.SQLUpdateSeasonDates(newstartdate, newenddate);

            //Response.Write("<script>$(Function(){ $('seasonSaved').css('color', 'red');)};</script>");
            HtmlGenericControl sparat = new HtmlGenericControl("p");
            sparat.Attributes.Add("id", "saveSeason");
            sparat.InnerHtml = "Ändringar sparade";
            tmp.Controls.Add(sparat);

        }
        

        //Öppna och stäng SÄSONG nedan

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            lblStartdatum.Text = Calendar1.SelectedDate.ToString("yyyy-MM-dd");
            string a = Calendar1.SelectedDate.ToString();
        }

        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
            lblSlutdatum.Text = Calendar2.SelectedDate.ToString("yyyy-MM-dd");
            string b = Calendar2.SelectedDate.ToString();
        }





        //Öppna och stäng DAGAR nedan

        protected void Calendar3_SelectionChanged(object sender, EventArgs e)
        {
            lblfirstclosedday.Text = Calendar3.SelectedDate.ToString("yyyy-MM-dd");
            string a = Calendar1.SelectedDate.ToString();
        }

        protected void Calendar4_SelectionChanged(object sender, EventArgs e)
        {
            lbllastclosedday.Text = Calendar3.SelectedDate.ToString("yyyy-MM-dd");
            string a = Calendar1.SelectedDate.ToString();
        }




        //Stäng knapp
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            string firstclosed = Calendar3.SelectedDate.ToString("yyyy-MM-dd");
            string lastclosed = Calendar4.SelectedDate.ToString("yyyy-MM-dd");

            DateTime newfirstclosed = Convert.ToDateTime(firstclosed + " " + dropfirstclose.SelectedValue.ToString());
            DateTime newlastclosed = Convert.ToDateTime(lastclosed + " " + droplastclose.SelectedValue.ToString());

            bool deleteclose = true;

            InsertOrDelteClosingCourse(newfirstclosed, newlastclosed, deleteclose);
            updateclosinglist();
        }

        private void InsertOrDelteClosingCourse(DateTime newfirstclosed, DateTime newlastclosed, bool addclosingtime)
        {

            if (!addclosingtime)
            {
                // Kod för att radera stängtider
                string closinginfo = listcloseddays.SelectedItem.ToString();
                string selected = "";

                foreach (char c in closinginfo)
                {
                    if (c == Convert.ToChar(":"))
                    {
                        break;
                    }
                    selected += c.ToString();                   
                }
                string branamn = "";
                String sql = "DELETE FROM closed WHERE closedid = '"+ selected +"'" ;
                ToolBox.SQL_NonParamCommand(sql, ref branamn);
            }
            else
            {
                // Kod för att läggatill stängnings tider, tabort krockande bokningar, aviseringar
                Postgress p = new Postgress();

                string sql = "INSERT INTO closed (course, startclose, endclose) ";
                sql += "VALUES('1', '" + newfirstclosed + "', '" + newlastclosed + "')";

                string resultmessage = "";
                ToolBox.SQL_NonParamCommand(sql, ref resultmessage);

                //Här tar vi bort bokningar inom spannet banan ska vara stängd.
                int counter = 0;
                while (newfirstclosed.AddDays(counter)<=newlastclosed)                                                      //Denna loop körs tills att loopens akutella dag är samma som sista stängningsdagen.
                {
                    DailyBookings dailybookings = new DailyBookings(newfirstclosed.AddDays(counter));                       //Hämtar alla bokningar för loppens aktuella dag.

                    if (counter == 0)                                                                                       //Kollar ifall akutelltdatum är den första dagen i spannet. 
                    {
                        foreach (Booking item in dailybookings.BookingsPerSpecifiedDate)                                    //Loopar igenom alla bokningar för den aktuella dagen.
                        {
                            DateTime tmp = newfirstclosed.Date + item.BookingTime.TimeOfDay;                                //Slår ihop datum och tid. En temporär variabel skapas som Datetime för att kunna jämföra med newfirstclosed. Egentligen bara intresserade av tiden på den första dagen i spannet.
                            if (tmp >= newfirstclosed)                                                                       //Klockslagen jämförs mot varandra. Tmp = aktuell bokningstid, kollar om tmp är mer än tiden i newfirstclosed
                            {
                               BookingSchedule.deletePlayerFromBooking("true", "000" , item.BookingId.ToString(), item.BookingTime.ToShortTimeString());          //Om tmp-tiden är mer än newfirstclosed så skall hela bokningen tas bort.
                            }
                        }
                    }
                    else if (newfirstclosed.AddDays(counter) == newlastclosed)
                    {
                        foreach (Booking item in dailybookings.BookingsPerSpecifiedDate)                                    //Loopar igenom alla bokningar för den aktuella dagen.
                        {
                            DateTime tmp = newlastclosed.Date + item.BookingTime.TimeOfDay;                                 //Slår ihop datum och tid. En temporär variabel skapas som Datetime för att kunna jämföra med newfirstclosed. Egentligen bara intresserade av tiden på den första dagen i spannet.
                            if (tmp <= newlastclosed)                                                                        //Klockslagen jämförs mot varandra. Tmp = aktuell bokningstid, kollar om tmp är mer än tiden i newfirstclosed
                            {
                                BookingSchedule.deletePlayerFromBooking("true", "000", item.BookingId.ToString(), item.BookingTime.ToShortTimeString());          //Om tmp-tiden är mer än newfirstclosed så skall hela bokningen tas bort.
                            }
                        }
                    }
                    else
                    {
                        DateTime tmp2 = new DateTime();                                                                     //Temporär variabel för att jämföra mot bokningstid, hålla koll på om akutell bokning i loopen ingår i föregående bokning.
                        foreach (Booking item in dailybookings.BookingsPerSpecifiedDate)                                    //Loopar igenom alla bokningar för den aktuella dagen.
                        {
                            DateTime tmp = newfirstclosed.AddDays(counter).Date + item.BookingTime.TimeOfDay;                                 //Slår ihop datum och tid. En temporär variabel skapas som Datetime för att kunna jämföra med newfirstclosed. Egentligen bara intresserade av tiden på den första dagen i spannet.

                            if (tmp != tmp2)
                            {
                                tmp2 = tmp;
                                
                            BookingSchedule.deletePlayerFromBooking("true", "000", item.BookingId.ToString(), item.BookingTime.ToShortTimeString());              //Om tmp-tiden är mer än newfirstclosed så skall hela bokningen tas bort.
                                
                            }                           
                        }
                    }
                    counter++;
                }              
            }
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            string firstclosed = Calendar3.SelectedDate.ToString("yyyy-MM-dd");
            string lastclosed = Calendar4.SelectedDate.ToString("yyyy-MM-dd");

            DateTime newfirstclosed = Convert.ToDateTime(firstclosed + " " + dropfirstclose.SelectedValue.ToString());
            DateTime newlastclosed = Convert.ToDateTime(lastclosed + " " + droplastclose.SelectedValue.ToString());

            bool deleteclose = false;

            InsertOrDelteClosingCourse(newfirstclosed, newlastclosed, deleteclose);
            updateclosinglist();
        }
    }
}
