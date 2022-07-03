using System;
using System.Collections.Generic;
using System.Text;

namespace HomeschoolApp.Models
{
    public class Activity
    {
        int Id { get; set; }
        string Title { get; set; }
        DateTime Date { get; set; }
        DateTime TimeStarted { get; set; }
        int DurationMinutes { get; set; }
        string Location { get; set; }
        bool Completed { get; set; }
        string Description { get; set; }
        string Notes { get; set; }

        List<int> Students { get; set; }
        List<LearningAreas> Subjects { get; set; }
        List<int> Photos { get; set; }
        List<int> Documents { get; set; }
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
