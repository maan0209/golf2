using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

namespace Golf2
{
    public partial class Mail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void sendBtn_Click(object sender, EventArgs e)
        {
            try { 
            
            MailMessage message = new MailMessage(toTbx.Text,fromTbx.Text,subjectTbx.Text,bodyTbx.Text);
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587);
            client.EnableSsl = true;

                //I paranteserna nedan fick jag hårdkoda in min egna mail och lösen du funkade det men tar bort det nu när jag checkar in =)
            client.Credentials = new System.Net.NetworkCredential();
            client.Send(message);
            Status.Text = "mailet är skickat";
            }

            catch(Exception ex)
            {
                Status.Text = ex.StackTrace;

            }


        }
        
    }
}