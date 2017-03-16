using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Golf2
{
    public partial class Master1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }


        // Tar de uppgifterna som fyllts i gällande golf-id och password och letar i databasens tabell account efter golfid och password som matchar. 
        // Vid träff så räknas obj upp och if satsen blir aktuell och Session[golfid] sätts till den träff som fanns.
        // Response.Redirect måste ändras till korrekt inloggad sida samt funktion för att skriva ut när det är felaktiga uppgifter.
        protected void Log_In_Click(object sender, EventArgs e)
        {
            Postgress p = new Postgress();
            p.cmd = new Npgsql.NpgsqlCommand("select count(*) from account where golfid = '" + golf_id.Text + "' AND password = '" + password.Text + "'", p.conn);

            p.cmd.Connection = p.conn;
            int obj = Convert.ToInt32(p.cmd.ExecuteScalar());           /* vi måste även hantera kontonamn med bokstäver, med tanke på personalkonton om de ej ska listas som vanliga golfidn - Martin N*/
            if(obj > 0)
            {
                Session["golfid"] = golf_id.Text;
                Response.Redirect("inloggad.aspx");
                p.conn.Close();
            }
            else
            {
                Response.Write("<script>alert('Felaktigt användarnamn och lösenord')</script>");
                p.conn.Close();
            }
            
            
            
            
        }
    }
}