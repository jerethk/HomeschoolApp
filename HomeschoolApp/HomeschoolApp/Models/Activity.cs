using System;
using System.Collections.Generic;
using System.Text;

namespace HomeschoolApp.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string TimeStarted { get; set; }
        public int DurationMinutes { get; set; }
        public string LearningAreas { get; set; }
        public string Location { get; set; }
        public bool IsCompleted { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }

        public bool IsDeleted { get; set; }

        List<int> Students { get; set; }
        List<int> Photos { get; set; }
        List<int> Documents { get; set; }

        public Activity()
        {
            // Set default values
            Title = "";
            Date = DateTime.Now.ToString();
            TimeStarted = "";
            DurationMinutes = 0;
            LearningAreas = "";
            Location = "";
            IsCompleted = true;
            Description = "";
            Notes = "";
            IsDeleted = false;

            Students = new List<int>();
            Photos = new List<int>();
            Documents = new List<int>();
        }
    }
}


