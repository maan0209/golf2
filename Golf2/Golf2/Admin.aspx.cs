using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Golf2
{
    public partial class Admin : System.Web.UI.Page
    {
        Postgress db = new Postgress();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void saveSeason_Click(object sender, EventArgs e)
        {
            string Startdate = Calendar1.SelectedDate.ToString("MM-dd-yyyy");
            string Enddate = Calendar2.SelectedDate.ToString("MM-dd-yyyy");

            DateTime newstartdate = Convert.ToDateTime(Startdate);
            DateTime newenddate = Convert.ToDateTime(Enddate);
                    
            db.SQLUpdateSeasonDates(newstartdate, newenddate);

        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            lblStartdatum.Text = Calendar1.SelectedDate.ToString("MM-dd-yyyy");
            string a = Calendar1.SelectedDate.ToString();
        }

        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
            lblSlutdatum.Text = Calendar2.SelectedDate.ToString("MM-dd-yyyy");
            string b = Calendar2.SelectedDate.ToString();
        }


    }
}