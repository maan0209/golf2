using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
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
        /// Hämta email från 'person' i databas utifrån golfid
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string SQLGetEmail(string sql, string golfid)
        {
            try
            {
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("golfid", golfid);
                string email = Convert.ToString(cmd.ExecuteScalar());

                return email;
            }

            catch (NpgsqlException ex)
            {
                string exm = ex.Message;
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// hämtar golfid utifrån bookingid i included
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="bookingid"></param>
        /// <returns></returns>
        public string SQLGetGolfidIncluded(string sql, int bookingid, int includedid)
        {
            try
            {
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("bookingid", bookingid);
                cmd.Parameters.AddWithValue("includedid", includedid);
                string golfid = Convert.ToString(cmd.ExecuteScalar());

                return golfid;
            }

            catch (NpgsqlException ex)
            {
                string exm = ex.Message;
                return "";
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// hämta includedid och golfsid utifrån bookingid
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="bookingid"></param>
        /// <returns></returns>
        public BindingList<Booking> SQLGetIncludedIds(string sql, int bookingid)
        {
            try
            {
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("bookingid", bookingid);
                dr = cmd.ExecuteReader();

                BindingList<Booking> includedids = new BindingList<Booking>();
                Booking included;

                while (dr.Read())
                {
                    included = new Booking()
                    {
                        includedid = (int)dr["includedid"],
                        GolfId = dr["golfid"].ToString(),
                    };
                    includedids.Add(included);
                }
                dr.Close();
                return includedids;

            }
            catch (NpgsqlException ex)
            {
                string exm = ex.Message;
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Metod för att skicka kommando till databasen
        /// Stöder ej parametrar/injections
        /// Returnerar "ok" om kommandot gick igenom visavi ett 
        /// felmeddelande i ett motsatt  scenario.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string SqlNonQuery(string sql)
        {
            string svar;
            try
            {
                cmd = new NpgsqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return svar = "ok";
            }
            catch (NpgsqlException ex)
            {
                conn.Close();
                return svar = ex.ToString();
            }
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
        public int SQLCheckDateAndTime(string sql, DateTime dt, int timeid, string owner)
        {
            try
            {
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("bookingdate", dt);
                cmd.Parameters.AddWithValue("timeid", timeid);
                cmd.Parameters.AddWithValue("owner", owner);

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

        /// <summary>
        /// Kontrollera om entry finns i 'included'
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dt"></param>
        /// <param name="timeid"></param>
        /// <param name="golfid"></param>
        /// <returns></returns>
        public string SQLCheckIncluded(string sql, DateTime dt, int timeid, string golfid)
        {
            try
            {
                conn.Open();
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("bookingdate", dt);
                cmd.Parameters.AddWithValue("timeid", timeid);
                cmd.Parameters.AddWithValue("golfid", golfid);

                string exists = Convert.ToString((cmd.ExecuteScalar()));

                return exists;
            }
            catch (NpgsqlException ex)
            {
                return "ex";
            }
            finally
            {

                conn.Close();
            }

        }

        /// <summary>
        /// Kontrollera om bokning redan finns för golfid på samma dag
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="golfid"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public string SQLCheckIfBooked(string sql, string golfid, DateTime date)
        {
            try
            {
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("golfid", golfid);
                cmd.Parameters.AddWithValue("date", date);

                string exists = Convert.ToString((cmd.ExecuteScalar()));

                return exists;
            }
            catch (NpgsqlException ex)
            {
                return "";
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

        public void ShowSeasondates(DateTime Startdate, DateTime Enddate)
        {
            try
            {
                string sql = "SELECT startdate, enddate FROM course"; 

                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("Startdate", Startdate);
                cmd.Parameters.AddWithValue("Enddate", Enddate);

                NpgsqlDataReader dr = cmd.ExecuteReader();

            }
            catch (NpgsqlException ex)
            {
            }
            finally
            {
                conn.Close();
            }
        } 

        public BindingList<string> SQLGetGolfidsClosed(string sql, DateTime date, string time)
        {

            TimeSpan time2 = TimeSpan.Parse(time);

            try
            {
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("bookingdate", date.Date);
                cmd.Parameters.AddWithValue("time", time2);
                dr = cmd.ExecuteReader();

                BindingList<string> golfids = new BindingList<string>();

                while (dr.Read())
                {
                      golfids.Add(dr["golfid"].ToString());
                }

                dr.Close();
                return golfids;

            }
            catch (NpgsqlException ex)
            {
                string exm = ex.Message;
                return null;
            }
            finally
            {
                conn.Close();
            }
        }


        //public void SQLInsertSeasonDates (DateTime Startdate, DateTime Enddate)
        //{
        //    try
        //    {
        //    string sql = "INSERT INTO course(startdate, enddate) VALUES (@Startdate, @Enddate)";

        //    cmd = new NpgsqlCommand(sql, conn);
        //    cmd.Parameters.AddWithValue("Startdate", Startdate);
        //    cmd.Parameters.AddWithValue("Enddate", Enddate);
        //    }
        //    catch(NpgsqlException ex)
        //    {

        //    }
        //    finally
        //    {
        //    conn.Close();
        //    }
        //}


        //public DataTable getseasondates(DateTime Startdate, DateTime Enddate)
        //{
        //        string sql = "SELECT startdate AND enddate FROM course";

        //        cmd = new NpgsqlCommand(sql, conn);
        //        dr = cmd.ExecuteReader();

        //    return table;
        //    }
        //}











        //public DataTable getaccount(string user, string pass)
        //{
        //    string sql = "select * from account where golfid = '" + user + "' AND password = '" +pass+"'";

        //    cmd = new NpgsqlCommand(sql, conn);
        //    dr = cmd.ExecuteReader();

        //    return table;






        //}

    }
}