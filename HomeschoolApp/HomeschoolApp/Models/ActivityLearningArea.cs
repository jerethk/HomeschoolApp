using System;
using System.Collections.Generic;
using System.Text;

namespace HomeschoolApp.Models
{
    // This class represents records from the table which associates Activities with Learning Areas
    public class ActivityLearningArea
    {
        public int Id { get; set; }
        public int Activity { get; set; }
        public int LearningArea { get; set; }
    }
}
