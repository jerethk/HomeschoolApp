using System;
using System.Collections.Generic;
using System.Text;

namespace HomeschoolApp.Models
{
    public class Student
    {
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime Dob { get; set; }
        int? Picture { get; set; }
        int YearLevel { get; set; }
        string Notes { get; set; }
    }
}
