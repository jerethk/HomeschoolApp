using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HomeschoolApp.Models;
using HomeschoolApp.Services;
using System.Collections.Generic;

namespace HomeschoolApp.Views
{
    public partial class MainPage : ContentPage
    {
        private List<Student> studentList;
        private Student selectedStudent;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            string errorString = "";
            DataAccess.createSchema1(out errorString);
            //DisplayAlert("", errorString, "ok");

            studentList = DataAccess.QueryAllStudents(out errorString);

            if (studentList != null)
            {
                feedback.Text = $"Count {studentList.Count} " + errorString;
                pickerStudent.ItemsSource = studentList;
                pickerStudent.ItemDisplayBinding = new Binding("FirstName");
                //pickerStudent.SelectedIndex = 0;
            }
            else
            {
                feedback.Text = "null " + errorString;
            }
        }

        private void onPickerStudentSelectedIndexChanged(object sender, EventArgs e)
        {
            if (pickerStudent.SelectedIndex >= 0)
            {
                selectedStudent = (Student)pickerStudent.SelectedItem;

                string errorMessage = "";
                List<Activity> activityList = DataAccess.QueryActivitiesByStudent(selectedStudent.Id, out errorMessage);
                collectionViewActivities.ItemsSource = activityList;
            }
            else
            {
                collectionViewActivities.ItemsSource = null;
            }
        }

        private async void onBtnEditStudentClicked(object sender, EventArgs e)
        {
            int id = -1;
            
            if (selectedStudent != null)
            {
                id = selectedStudent.Id;
            }

            //await DisplayAlert("", nameof(StudentEditor) + $"?student={id}", "ok");
            await Shell.Current.GoToAsync(nameof(StudentEditor) + $"?student={id}");
        }

        private async void onBtnActivitiesClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ActivitiesPage));
        }

    }
}