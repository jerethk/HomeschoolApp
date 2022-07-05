using System;
using System.Collections.Generic;
using System.Text;

namespace HomeschoolApp.Models
{
    // This class represents records from the table which associates students and activities
    public class ActivityStudent
    {
        public int Id { get; set; }
        public int Activity { get; set; }
        public int Student { get; set; }
    }
}
