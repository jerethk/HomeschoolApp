using System;
using System.Collections.Generic;
using System.Text;

namespace HomeschoolApp.Models
{
    // Class used for querying the sqlite_sequence table
    public class SqliteSeq
    {
        public string Name { get; set; }
        public string Seq { get; set; }

        public SqliteSeq()
        {

        }
    }
}
