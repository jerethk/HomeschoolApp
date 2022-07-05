using System;
using System.Collections.Generic;
using System.Text;

namespace HomeschoolApp.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public DateTime TimeStarted { get; set; }
        public int DurationMinutes { get; set; }
        public string Location { get; set; }
        public bool IsCompleted { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }

        bool IsDeleted { get; set; }

        List<int> Students { get; set; }
        List<LearningAreas> Subjects { get; set; }
        List<int> Photos { get; set; }
        List<int> Documents { get; set; }

        public Activity()
        {
            // Set default values
            Title = "";
            Date = DateTime.Now;
            TimeStarted = DateTime.Now;
            DurationMinutes = 0;
            Location = "";
            IsCompleted = true;
            Description = "";
            Notes = "";
            IsDeleted = false;

            Students = new List<int>();
            Subjects = new List<LearningAreas>();
            Photos = new List<int>();
            Documents = new List<int>();
        }
    }

    public enum LearningAreas
    {
        English,
        Maths,
        Science,
        Humanities,
        Arts,
        Tech,
        HealthPE,
        Languages
    }
}
