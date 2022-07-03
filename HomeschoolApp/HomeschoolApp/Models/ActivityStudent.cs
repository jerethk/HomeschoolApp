using System;
using System.Collections.Generic;
using System.Text;

namespace HomeschoolApp.Models
{
    // This class represents records from the table which associates students and activities
    public class ActivityStudent
    {
        int Id { get; set; }
        int Activity { get; set; }
        int Student { get; set; }
    }
}
