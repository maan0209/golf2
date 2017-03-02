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

        //public DataTable getaccount(string user, string pass)
        //{
        //    string sql = "select * from account where golfid = '" + user + "' AND password = '" +pass+"'";

        //    cmd = new NpgsqlCommand(sql, conn);
        //    dr = cmd.ExecuteReader();

        //    return table;
            
           
            
           


        //}

    }
}