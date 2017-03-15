using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Data;
using System.ComponentModel;

namespace Golf2
{
    public partial class Mail : System.Web.UI.Page
    {
        BindingList<string> EmailList = new BindingList<string>();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void sendBtn_Click(object sender, EventArgs e)
        {
         
        }

        public void SendMail(DateTime date, DateTime time, string typeOfMail, BindingList<string> golfIds)
        {

            GetEmails(golfIds);

            string notification;

            if (typeOfMail == "booking")
            {
                foreach (string s in EmailList)
                {
                    subjectTbx.Text = "Booking";
                    notification = "Du är inbokad i en boll klockan " + time + " den " + date;
                    bodyTbx.Text = notification;

                    MailMessage message = new MailMessage("Golfklubben", s);
                    message.IsBodyHtml = true;

                    SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587);
                    client.EnableSsl = true;

                    client.Credentials = new System.Net.NetworkCredential("golfklubben_halslaget@outlook.com", "Golfbil123321");
                    client.Send(message);
                    Status.Text = "mailet är skickat";
                }
            }

            else if (typeOfMail == "cancellation")
            {
                foreach (string s in EmailList)
                {
                    subjectTbx.Text = "Cancellation";
                    notification = "Din bokning för " + date + " klockan " + time + " har blivit avbokad";
                    bodyTbx.Text = notification;

                    MailMessage message = new MailMessage(fromTbx.Text, toTbx.Text);
                    message.IsBodyHtml = true;

                    SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587);
                    client.EnableSsl = true;

                    client.Credentials = new System.Net.NetworkCredential("golfklubben_halslaget@outlook.com", "Golfbil123321");
                    client.Send(message);
                    Status.Text = "mailet är skickat";
                }
            }

            try
            {
                MailMessage message = new MailMessage(fromTbx.Text, toTbx.Text);
                message.IsBodyHtml = true;

                SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587);
                client.EnableSsl = true;

                client.Credentials = new System.Net.NetworkCredential("golfklubben_halslaget@outlook.com", "Golfbil123321");
                client.Send(message);
                Status.Text = "mailet är skickat";
            }

            catch (Exception ex)
            {
                Status.Text = ex.StackTrace;
            }
        }

        private BindingList<string> GetEmails(List<string> golfID)
        {
            string sql = "SELECT email FROM person WHERE golfid= " + golfID;
            DataTable Table = new DataTable();
            ToolBox.SQL_NonParam(sql,ref Table);
             EmailList = new BindingList<string>();
            foreach(DataRow item in Table.Rows)
            {
                EmailList.Add(item["email"].ToString());
            }
            return EmailList;
        }


        //      try
        //    {
        //        MailMessage message = new MailMessage(fromTbx.Text, toTbx.Text, subjectTbx.Text, bodyTbx.Text);
        //message.IsBodyHtml = true;

        //        SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587);
        //client.EnableSsl = true;

        //        client.Credentials = new System.Net.NetworkCredential(User.Text, Password.Text);
        //        client.Send(message);
        //        Status.Text = "mailet är skickat";
        //    }

        //    catch (Exception ex)
        //    {
        //        Status.Text = ex.StackTrace;
        //    }
    }
}