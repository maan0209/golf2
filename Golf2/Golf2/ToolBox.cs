using System;
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
        #endregion

        #region ############### SQL OPERATIONS ###############
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
        #endregion
    }
}