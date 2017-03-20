using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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



            try
            {
                MailMessage message = new MailMessage(fromTbx.Text, toTbx.Text, subjectTbx.Text, bodyTbx.Text);
                message.IsBodyHtml = true;

                SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587);
                client.EnableSsl = true;

                client.Credentials = new System.Net.NetworkCredential("golfklubben_halslaget", "Golfbil123321");
                client.Send(message);
                Status.Text = "mailet är skickat";
            }

            catch (Exception ex)
            {
                Status.Text = ex.StackTrace;
            }
        }

        /// <summary>
        /// Metoden som skickar aviseringar om bokning/avbokning
        /// </summary>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <param name="typeOfMail"></param>
        /// <param name="golfIds"></param>
        public void SendMail(DateTime date, string time, string typeOfMail, BindingList<string> golfIds)
        {
            foreach (string item in golfIds)
            {
                if (item != "Gäst")
                {
                    string mail = GetEmail(item);
                    EmailList.Add(mail);
                }          
            }
            
            string notification;

            try
            {
                if (typeOfMail == "booking")
                {
                    foreach (string email in EmailList)
                    {
                        MailMessage message = new MailMessage("golfklubben_halslaget@outlook.com", email);
                        message.IsBodyHtml = true;
                        message.Subject = "Booking";
                        notification = "Du är inbokad i en boll klockan " + time + " den " + date.ToShortDateString();
                        message.Body = notification;

                        SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587);
                        client.EnableSsl = true;

                        client.Credentials = new System.Net.NetworkCredential("golfklubben_halslaget@outlook.com", "Golfbil123321");
                        client.Send(message);
                        //Status.Text = "mailet är skickat";
                    }
                }


                else if (typeOfMail == "cancellation")
                {
                    foreach (string email in EmailList)
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
                        //Status.Text = "mailet är skickat";
                    }
                }
            }

            catch { }
        }

        /// <summary>
        /// Hämtar emails utifrån golfids
        /// </summary>
        /// <param name="golfIDs"></param>
        /// <returns></returns>
        private string GetEmail(string golfid)
        {
            Postgress p = new Postgress();
            string sql = "SELECT email FROM person WHERE golfid= @golfid";
            string mail = p.SQLGetEmail(sql, golfid);
            return mail;
        }


    }
}