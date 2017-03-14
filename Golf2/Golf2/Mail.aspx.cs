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
            
            //I paranteserna nedan fick jag hårdkoda in min egna mail och lösen då funkade det men tar bort det nu när jag checkar in =)
            client.Credentials = new System.Net.NetworkCredential();
            client.Send(message);
            Status.Text = "mailet är skickat";
            }

            catch(Exception ex)
            {
                Status.Text = ex.StackTrace;
            }

        }

        public void MailMessage(DateTime date, DateTime time, string typeOfMail, List<string> mailAddresses)
        {
            string notification;

            if (typeOfMail == "booking")
            {
                foreach(string s in mailAddresses)
                {
                    subjectTbx.Text = "Booking";
                    notification = "Du är inbokad i en boll klockan " + time + " den " + date;                 
                    bodyTbx.Text = notification;                   
                }    
            }
        
            else if (typeOfMail == "cancellation")
            {
                foreach (string s in mailAddresses)
                {
                    subjectTbx.Text = "Cancellation";
                    notification = "Din bokning för " + date + " klockan " + time + " har blivit avbokad";
                    bodyTbx.Text = notification;                    
                }              
            }          
        }
    }
}