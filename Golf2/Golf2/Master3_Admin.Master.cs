using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Golf2
{
    public partial class Master3_Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["golfid"] == Session[""])
            {
                Response.Redirect("index.aspx");
            }
            else
                Inloggat_ID.Text = Session["golfid"].ToString();
        }
    }
}