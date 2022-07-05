using System;
using System.Collections.Generic;
using System.Text;

namespace HomeschoolApp.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public Sex Sex { get; set; }
        public int YearLevel { get; set; }
        public int Picture { get; set; }
        public string Notes { get; set; }

        bool IsDeleted { get; set; }

        public Student()
        {
        }

        public Student(string firstName, string lastName, Sex sex, int yearLevel)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Dob = DateTime.Now;
            this.Sex = sex;
            this.Picture = -1;
            this.YearLevel = yearLevel;
            this.Notes = "";
            this.IsDeleted = false;
        }
    }

    public enum Sex
    {
        M,
        F
    }
}
