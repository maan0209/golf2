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
    }
}