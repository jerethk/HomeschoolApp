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

                dbConnection.Execute("drop table 'students'");

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
                    "PRIMARY KEY('id' AUTOINCREMENT)" +
                    ");";

                dbConnection.Execute(queryString);

                queryString = "INSERT INTO students (firstName, lastName, dob, sex, picture, yearLevel, notes) VALUES('Luke', 'Skywalker', datetime('1999-06-07'), 'M', 0, 2, 'Born on Tatooine and his dad is Vader'); ";
                dbConnection.Execute(queryString);
                queryString = "INSERT INTO students (firstName, lastName, dob, sex, picture, yearLevel, notes) VALUES('Leia', 'Organa', datetime('1999-06-07 00:00:00.000'), 'F', 0, 6, 'A princess'); ";
                dbConnection.Execute(queryString);
                queryString = "INSERT INTO students (firstName, lastName, dob, sex, picture, yearLevel, notes) VALUES('Han', 'Solo', datetime('1994-02-07 00:00:00.000'), 'M', 0, 0, 'Friends with Chewbacca who is a Wookie'); ";
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

        // STUDENTS  --------------------------------------------------------------------------

        public static List<Student> queryAllStudents(out string error)
        {
            error = "no error";

            using (dbConnection = new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DBFILENAME)))
            {
                try
                {
                    string queryString = "SELECT * from students";
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

        public static Student queryStudentById(int id, out string error)
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

        public static void UpdateStudent(Student student)
        {

        }

        public static void AddNewStudent(Student student)
        {

        }

    }
}
