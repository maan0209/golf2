﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Golf2
{
    public static class ToolBox
    {

        #region ############### FIELDS ##############

        private static Postgress newPostgress;

        #endregion

        #region ############### SQL OPERATIONS ###############
        /// <summary>
        /// Databasanrop som ej använder parametrar.
        ///     1. Tar emot en string och en ref av datatyp DataTable.
        ///     2. Ropar på databasen och uppdaterar ref'en i ursprungsplatsen.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="answerTable"></param>
        public static void SQL_NonParam(string sql, ref DataTable answerTable)
        {
            newPostgress = new Postgress();
            answerTable = newPostgress.sqlquestion(sql);
        }

        /// <summary>
        /// Databasanrop som ej använder parametrar
        ///     1. Tar emot en string och en ref av datatyp string
        ///     2. Ropar på databasen och skickar iväg sql-kommando
        ///     3. Tar emot ett "ok" från Postgres om allt gick väl, alternativt ett felmeddelande
        ///     4. Skickar vidare tillbaka meddelandet genom ref-variabeln
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="resultMessage"></param>
        public static void SQL_NonParamCommand(string sql, ref string resultMessage)
        {
            newPostgress = new Postgress();
            resultMessage = newPostgress.SqlNonQuery(sql);
        }
        #endregion

        #region ############### OTHER ###############
        /// <summary>
        /// Tar emot ett golfid och kontrollerar om den är en admin. 
        /// 1. Returnerar false om det inte är en admin 
        /// 2. Returnerar true om det är en admin
        /// </summary>
        /// <param name="isadmin"></param>
        public static void checkIfUserIsAdmin(ref bool isadmin, string golfid)
        {
            if (golfid.Contains("_"))
            {
                isadmin = false;
            }
            else
            {
                isadmin = true;
            }
        }

        /// <summary>
        /// 1. Metoden tar emot ett datum och en tid, drar av fem minuter på tiden
        /// 2. kontrollerar ifall nuvarande klocklag (obs! format hh:mm:ss) är mer än den mottagna justerade tiden
        /// 3. returnerar true om detta stämmer
        /// </summary>
        /// <param name="anyDate"></param>
        /// <param name="anyTime"></param>
        /// <returns></returns>
        public static bool timeLimitCheck(DateTime anyDate, string anyTime)
        {
            DateTime currentTime = DateTime.Now;
            DateTime timeLimitation = Convert.ToDateTime(anyDate.ToShortDateString() + " " + anyTime).AddMinutes(-5);
            bool currTimeIsMoreThanTimeLimit = (currentTime > timeLimitation) ? true : false;

            return currTimeIsMoreThanTimeLimit;
        }

        #endregion
    }
}