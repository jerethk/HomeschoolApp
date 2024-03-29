﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeschoolApp.Models;
using HomeschoolApp.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeschoolApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("IncomingId", "student")]
    public partial class StudentEditor : ContentPage
    {
        private List<Student> studentList;
        private Student selectedStudent;

        public string IncomingId { get; set; }

        public StudentEditor()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            string errorString;
            studentList = DataAccess.QueryAllStudents(out errorString);

            if (studentList != null)
            {
                CheckBoxNewStudent.IsChecked = false;
                pickerStudent.IsEnabled = true;

                // go to selected student (passed from main page)
                pickerStudent.ItemsSource = studentList;
                pickerStudent.ItemDisplayBinding = new Binding("FirstName");
                int studentId = Int32.Parse(IncomingId);
                int index = -1;
                for (int i = 0; i < studentList.Count; i++)
                {
                    if (studentList[i].Id == studentId)
                    {
                        index = i;
                        break;
                    }
                }

                pickerStudent.SelectedIndex = index;
                //label1.Text = IncomingId;
            }

            // If no students in database, user must create a new one
            if (studentList == null || studentList.Count == 0)
            {
                CheckBoxNewStudent.IsChecked = true;
            }
        }

        private void OnCheckBoxNewStudentChanged(object sender, CheckedChangedEventArgs e)
        {
            // If no students in database, user must create a new one
            if (studentList == null || studentList.Count == 0)
            {
                CheckBoxNewStudent.IsChecked = true;
            }

            if (CheckBoxNewStudent.IsChecked)
            {
                pickerStudent.IsVisible = false;
                
                // Clear all entries
                entryFirstName.Text = "";
                entryLastName.Text = "";
                pickerDob.Date = DateTime.Parse("2000-01-01");
                pickerSex.SelectedIndex = 0;
                pickerYearLevel.SelectedIndex = 0;
                //studentImage.
                editorNotes.Text = " ";
            } 
            else
            {
                UpdateFields();
                pickerStudent.IsVisible = true;
            }
        }

        private void OnPickerStudentSelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFields();
        }

        private async void OnBtnSaveClicked(object sender, EventArgs e)
        {
            bool isValid = ValidateEntries();
            if (isValid)
            {
                string errorString = "";

                if (CheckBoxNewStudent.IsChecked)
                {
                    // Add new student
                    Student newStudent = new Student();
                    newStudent.FirstName = entryFirstName.Text;
                    newStudent.LastName = entryLastName.Text;
                    newStudent.Dob = pickerDob.Date.ToString();
                    newStudent.Sex = (pickerSex.SelectedIndex == 0) ? Sex.M : Sex.F;
                    newStudent.YearLevel = pickerYearLevel.SelectedIndex;
                    newStudent.Notes = editorNotes.Text;
                    DataAccess.AddNewStudent(newStudent, out errorString);
                }
                else
                {
                    // Update student details
                    Student updatedStudent = new Student();
                    updatedStudent.Id = selectedStudent.Id;
                    updatedStudent.FirstName = entryFirstName.Text;
                    updatedStudent.LastName = entryLastName.Text;
                    updatedStudent.Dob = pickerDob.Date.ToString();
                    updatedStudent.Sex = (pickerSex.SelectedIndex == 0) ? Sex.M : Sex.F;
                    updatedStudent.YearLevel = pickerYearLevel.SelectedIndex;
                    updatedStudent.Notes = editorNotes.Text;

                    DataAccess.UpdateStudent(updatedStudent, out errorString);
                }

                label1.Text = errorString;
                await DisplayAlert("", "Done", "Ok");
                await Shell.Current.GoToAsync("..");
            }
        }

        private void UpdateFields()
        {
            if (pickerStudent.SelectedIndex >= 0)
            {
                selectedStudent = (Student)pickerStudent.SelectedItem;

                // Fill entries with student data
                entryFirstName.Text = selectedStudent.FirstName;
                entryLastName.Text = selectedStudent.LastName;
                pickerDob.Date = DateTime.Parse(selectedStudent.Dob);
                pickerSex.SelectedIndex = (selectedStudent.Sex == Sex.M) ? 0 : 1;
                pickerYearLevel.SelectedIndex = selectedStudent.YearLevel;
                //studentImage.
                editorNotes.Text = selectedStudent.Notes;

                label1.Text = selectedStudent.Dob;
            }
        }

        private bool ValidateEntries()
        {
            if (entryFirstName.Text == null || entryFirstName.Text == "")
            {
                DisplayAlert("", "First name cannot be empty", "ok");
                return false;
            }

            return true;
        }

        private void ondatechanged(object sender, DateChangedEventArgs e)
        {
            label1.Text = pickerDob.Date.ToString();
        }
    }
}