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



    }
}