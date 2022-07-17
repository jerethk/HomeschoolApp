using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HomeschoolApp.Models;

namespace HomeschoolApp.Services
{
    /// <summary>
    /// This class contains the methods for accessing the database
    /// </summary>
    public static class DataAccess
    {
        private const string DBFILENAME = "homeschoolapp.db";
        private static SQLiteConnection dbConnection;

        public static bool createDb()
        {
            try
            {
                dbConnection = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DBFILENAME));
            }
            catch (SQLiteException e)
            {
                string error = e.ToString();
                return false;
            }

            return true;
        }

        public static bool createSchema1(out string error)
        {
            bool returnValue = true;
            error = "no error";

            try
            {
                dbConnection = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DBFILENAME));

                //dbConnection.Execute("drop table 'students'");
                //dbConnection.Execute("drop table 'activities'");
                //dbConnection.Execute("drop table 'activities_students'");

                // Create Student table
                string queryString = "CREATE TABLE IF NOT EXISTS 'students' (" +
                    "'id' INTEGER," +
                    "'firstName' TEXT," +
                    "'lastName' TEXT," +
                    "'dob' TEXT," +
                    "'sex' TEXT," +
                    "'picture' INTEGER," +
                    "'yearLevel' INTEGER," +
                    "'notes' TEXT," +
                    "'isDeleted' INTEGER," +
                    "PRIMARY KEY('id' AUTOINCREMENT)" +
                    ");";

                dbConnection.Execute(queryString);

                //queryString = "INSERT INTO students (firstName, lastName, dob, sex, picture, yearLevel, notes) VALUES('Luke', 'Skywalker', '1999-06-07', 'M', 0, 2, 'Born on Tatooine and his dad is Vader'); ";
                //dbConnection.Execute(queryString); 
                //queryString = "INSERT INTO students (firstName, lastName, dob, sex, picture, yearLevel, notes) VALUES('Leia', 'Organa', '1999-06-07 00:00:00.000', 'F', 0, 6, 'A princess'); ";
                //dbConnection.Execute(queryString); 
                //queryString = "INSERT INTO students (firstName, lastName, dob, sex, picture, yearLevel, notes) VALUES('Han', 'Solo', '1994-02-07 00:00:00.000', 'M', 0, 0, 'Friends with Chewbacca who is a Wookie'); ";
                //dbConnection.Execute(queryString);

                // Create activities table
                queryString = "CREATE TABLE IF NOT EXISTS 'activities' (" +
                    "id INTEGER," +
                    "title TEXT," +
                    "date TEXT," +
                    "timeStarted TEXT," +
                    "durationMinutes INTEGER," +
                    "learningAreas TEXT," +
                    "location TEXT," +
                    "isCompleted INTEGER," +
                    "description TEXT," +
                    "notes TEXT," +
                    "isDeleted INTEGER," +
                    "PRIMARY KEY('id' AUTOINCREMENT)" +
                    ");";

                dbConnection.Execute(queryString);

                // Create Activities-Students table
                queryString = "CREATE TABLE IF NOT EXISTS 'activities_students' (" +
                    "id INTEGER," +
                    "activity INTEGER," +
                    "student INTEGER," +
                    "PRIMARY KEY('id' AUTOINCREMENT)" +
                    ");";
                dbConnection.Execute(queryString);
            }
            catch (SQLiteException e)
            {
                error = e.ToString();
                returnValue = false;
            }
            finally
            {
                dbConnection.Dispose();
            }

            return returnValue;
        }

        public static bool createSchema2(out string error)
        {
            bool returnValue = true;
            error = "no error";

            using (dbConnection = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DBFILENAME)))
            {
                try
                {

                }
                catch (SQLiteException e)
                {
                    error = e.ToString();
                    returnValue = false;
                }
            }

            return returnValue;
        }

        // STUDENTS  --------------------------------------------------------------------------

        public static List<Student> QueryAllStudents(out string error)
        {
            error = "no error";

            using (dbConnection = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DBFILENAME)))
            {
                try
                {
                    string queryString = "SELECT * FROM students";
                    var queryResult = dbConnection.Query<Student>(queryString);
                    return queryResult;
                }
                catch (SQLiteException e)
                {
                    error = e.ToString();
                    return null;
                }
            }
        }

        public static Student QueryStudentById(int id, out string error)
        {
            error = "no error";

            using (dbConnection = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DBFILENAME)))
            {
                try
                {
                    string queryString = $"SELECT * FROM students WHERE 'id'={id}";
                    var queryResult = dbConnection.Query<Student>(queryString);
                    
                    if (queryResult.Count == 1)
                    {
                        return queryResult[0];
                    }
                    else
                    {
                        error = "Unable to query student";
                        return null;
                    }
                }
                catch (SQLiteException e)
                {
                    error = "Exception: " + e.ToString();
                    return null;
                }
            }
        }

        public static bool UpdateStudent(Student student, out string error)
        {
            error = "no error";

            using (dbConnection = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DBFILENAME)))
            {
                try
                {
                    string sex = (student.Sex == Sex.M) ? "M" : "F";
                    string queryString = $"UPDATE students SET firstName = '{student.FirstName}', lastName = '{student.LastName}', DOB = '{student.Dob.ToString()}', sex = '{sex}', picture = {student.Picture}, yearLevel = {student.YearLevel}, notes='{student.Notes}' " +
                        $"WHERE id = {student.Id};";

                    dbConnection.Execute(queryString);
                }
                catch (SQLiteException e)
                {
                    error = e.ToString();
                    return false;
                }
            }

            return true;
        }

        public static bool AddNewStudent(Student student, out string error)
        {
            error = "no error";

            using (dbConnection = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DBFILENAME)))
            {
                try
                {
                    string sex = (student.Sex == Sex.M) ? "M" : "F";
                    string queryString = $"INSERT INTO students (firstName, lastName, dob, sex, picture, yearLevel, notes) " +
                        $"VALUES ('{student.FirstName}', '{student.LastName}', '{student.Dob}', '{sex}', {student.Picture}, {student.YearLevel}, '{student.Notes}');";
                    
                    dbConnection.Execute(queryString);
                }
                catch (SQLiteException e)
                {
                    error = e.ToString();
                    return false;
                }
            }

            return true;
        }


        // ACTIVITIES  --------------------------------------------------------------------------

        public static List<Activity> QueryAllActivities(out string error)
        {
            error = "";
            
            using (dbConnection = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DBFILENAME)))
            {
                try
                {
                    var activityList = dbConnection.Query<Activity>("SELECT * FROM activities");
                    return activityList;
                }
                catch (SQLiteException e)
                {
                    error = e.ToString();
                    return null;
                }
            }
        }

        public static Activity QueryActivityById(int id, out string error)
        {
            error = "";

            using (dbConnection = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DBFILENAME)))
            {
                try
                {
                    var queryResult = dbConnection.Query<Activity>($"SELECT * FROM activities WHERE id = {id}");
                    
                    if (queryResult.Count == 1)
                    {
                        return queryResult[0];
                    }
                    else
                    {
                        error = "Unable to query activity";
                        return null;
                    }
                }
                catch (SQLiteException e)
                {
                    error = e.ToString();
                    return null;
                }
            }

        }

        // Insert a new activity and return the activity's auto-generated id
        public static int AddNewActivity(Activity activity, out string error)
        {
            error = "no error";
            int id = -1;

            using (dbConnection = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DBFILENAME)))
            {
                try
                {
                    string queryString = $"INSERT INTO activities(title, date, timeStarted, durationMinutes, learningAreas, location, isCompleted, description, notes, isDeleted)" +
                        $"VALUES('{activity.Title}', '{activity.Date}', '{activity.TimeStarted}', {activity.DurationMinutes}, '{activity.LearningAreas}', '{activity.Location}', {activity.IsCompleted}, '{activity.Description}', '{activity.Notes}', {activity.IsDeleted});";
                    dbConnection.Execute(queryString);

                    // retrieve the id from the sqlite_sequence table
                    queryString = "SELECT seq FROM sqlite_sequence WHERE name = 'activities'";
                    var queryResult = dbConnection.Query<SqliteSeq>(queryString);
                    id = Int32.Parse(queryResult[0].Seq);
                }
                catch (SQLiteException e)
                {
                    error = e.ToString();
                    return -1;
                }
            }

            return id;
        }

        public static bool AddActivityStudent(ActivityStudent activityStudent, out string error)
        {
            error = "no error";

            using (dbConnection = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DBFILENAME)))
            {
                try
                {
                    string queryString = $"INSERT INTO activities_students (activity, student) " +
                        $"VALUES ({activityStudent.Activity}, {activityStudent.Student})";
                    dbConnection.Execute(queryString);
                }
                catch (SQLiteException e)
                {
                    error = e.ToString();
                    return false;
                }
            }

            return true;
        }

    }
}
