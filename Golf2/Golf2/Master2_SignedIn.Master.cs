using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Golf2
{
    public partial class Master2_SignedIn : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["golfid"] == Session[""])
            {
                Response.Redirect("index.aspx");
            }
            else
                Inloggat_ID.Text = Session["golfid"].ToString();

        }

        protected void Log_out_Click(object sender, EventArgs e)
        {
            Session["golfid"] = Session[""];
            Response.Redirect("index.aspx");
        }
    }
}