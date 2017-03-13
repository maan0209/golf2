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

            try
            {
                cmd = new NpgsqlCommand(sql, conn);
                dr = cmd.ExecuteReader();
                table.Load(dr);
            }
            catch (NpgsqlException ex)
            {
                //DataColumn c1 = new DataColumn("Error");
                //DataColumn c2 = new DataColumn("ErrorMessage");

                //c1.DataType = Type.GetType("System.Boolean");
                //c2.DataType = Type.GetType("System.String");

                //table.Columns.Add(c1);
                //table.Columns.Add(c2);

                //DataRow rad = table.NewRow();
                //rad[c1] = true;
                //rad[c2] = ex.Message;
                //table.Rows.Add(rad);
            }
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
        public int SQLBooking(string sql, int timeid, DateTime bookingdate, string owner)
        {
            try
            {
                conn.Open();
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("bookingdate", bookingdate);
                cmd.Parameters.AddWithValue("courseid", "'1'");
                cmd.Parameters.AddWithValue("timeid", timeid);
                cmd.Parameters.AddWithValue("owner", owner);

                int bookingid = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
                return bookingid;
            }
            catch (NpgsqlException ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Lägg in golfidn kopplade till bokningsid från SQLBooking
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="golfid"></param>
        /// <param name="bookingid"></param>
        public void SQLbooking2(string sql, string golfid, int bookingid)
        {
            try
            {
                conn.Open();
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("golfid", golfid);
                cmd.Parameters.AddWithValue("bookingid", bookingid);
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
            
            }
            finally
            {
                conn.Close();
            }
            
        }

        /// <summary>
        /// Kontrollera om timeid redan finns för givet datum, för att avgöra INSERT eller UPDATE
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="bookingdate"></param>
        /// <param name="timeid"></param>
        /// <returns></returns>
        public int SQLCheckDateAndTime(string sql, DateTime dt, int timeid)
        {
            try
            {
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("bookingdate", dt);
                cmd.Parameters.AddWithValue("timeid", timeid);

                int exists = Convert.ToInt32((cmd.ExecuteScalar()));

                return exists;
            }
            catch (NpgsqlException ex)
            {
                return 2;
            }
            finally
            {
                
                conn.Close();
            }
            
        }





        public void SQLUpdateSeasonDates(DateTime Startdate, DateTime Enddate)
        {
            try {
                string sql = "UPDATE course SET startdate = @Startdate, enddate = @Enddate";

            cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("Startdate", Startdate);
            cmd.Parameters.AddWithValue("Enddate", Enddate);
            cmd.ExecuteNonQuery();
            }
            catch(NpgsqlException ex)
            {

            }
            finally
            {
                conn.Close();
            }
        }


        public void SQLInsertSeasonDates (DateTime Startdate, DateTime Enddate)
        {
            try
            {
            string sql = "INSERT INTO course(startdate, enddate) VALUES (@Startdate, @Enddate)";

            cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("Startdate", Startdate);
            cmd.Parameters.AddWithValue("Enddate", Enddate);
            }
            catch(NpgsqlException ex)
            {

            }
            finally
            {
            conn.Close();
            }
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