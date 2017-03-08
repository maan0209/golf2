using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Golf2
{
    public class Postgress
    {
        public NpgsqlConnection conn;
        public NpgsqlCommand cmd;
        public NpgsqlDataReader dr;
        public DataTable table;

        //Metod för att ansluta till databasen
        public Postgress()
        {
            conn = new NpgsqlConnection("Server=webblabb.miun.se;Port=5432;Database=dsu_g2;User Id=dsu_g2;Password=golfbil;Database=dsu_g2;SslMode=Require");

            try
            {
                conn.Open();
            }
            catch (NpgsqlException ex)
            {
                string message = ex.Message;
            }

            table = new DataTable();
        }

        //Metod för att avsluta anslutningen till databasen
        public void Close()
        {
            conn.Close();
        }

        //Metod för sql fråga
        public DataTable sqlquestion(string sql)
        {
            cmd = new NpgsqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            table.Load(dr);
            conn.Close();
            return table;
        }

        /// <summary>
        /// Lägg till tidsbokning för boll, returnera bokningsid
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="timeid"></param>
        /// <param name="bookingdate"></param>
        /// <returns></returns>
        public int SQLBooking(string sql, int timeid, string bookingdate)
        {
            cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("bookingdate", bookingdate);
            cmd.Parameters.AddWithValue("courseid", "1");
            cmd.Parameters.AddWithValue("timeid", timeid);

            int bookingid = Convert.ToInt32(cmd.ExecuteScalar());

            return bookingid;
        }

        /// <summary>
        /// Lägg in golfidn kopplade till bokningsid från SQLBooking
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="golfid"></param>
        /// <param name="bookingid"></param>
        public void SQLbooking2(string sql, string golfid, int bookingid)
        {

            cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("golfid", golfid);
            cmd.Parameters.AddWithValue("bookingid", bookingid);
            cmd.ExecuteNonQuery();

            
        }

        /// <summary>
        /// Kontrollera om timeid redan finns för givet datum, för att avgöra INSERT eller UPDATE
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="bookingdate"></param>
        /// <param name="timeid"></param>
        /// <returns></returns>
        public int SQLCheckDateAndTime(string sql, string bookingdate, int timeid)
        {
            cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("bookingdate", bookingdate);
            cmd.Parameters.AddWithValue("timeid", timeid);

            int exists = (int)(cmd.ExecuteScalar());

            return exists;
        }




        //public DataTable getaccount(string user, string pass)
        //{
        //    string sql = "select * from account where golfid = '" + user + "' AND password = '" +pass+"'";

        //    cmd = new NpgsqlCommand(sql, conn);
        //    dr = cmd.ExecuteReader();

        //    return table;
            
           
            
           


        //}

    }
}