using System;
using System.Collections.Generic;
using System.Text;

namespace HomeschoolApp.Models
{
    // This class represents records from the table which associates Activities with Learning Areas
    public class ActivityLearningArea
    {
        int Id { get; set; }
        int Activity { get; set; }
        int LearningArea { get; set; }
    }
}
