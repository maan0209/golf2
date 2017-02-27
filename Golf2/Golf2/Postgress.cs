using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using System.Data;

namespace Golf2
{
    public class Postgress
    {
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private NpgsqlDataReader dr;
        private DataTable tabel;

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

            tabel = new DataTable();
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
            tabel.Load(dr);
            conn.Close();
            return tabel;
        }

    }
}