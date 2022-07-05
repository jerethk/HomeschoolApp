﻿using System;
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

            studentList = DataAccess.queryAllStudents(out errorString);

            if (studentList != null)
            {
                feedback.Text = $"Count {studentList.Count} " + errorString;
                pickerStudent.ItemsSource = studentList;
                pickerStudent.ItemDisplayBinding = new Binding("FirstName");
                pickerStudent.SelectedIndex = 0;
            }
            else
            {
                feedback.Text = "null " + errorString;
            }
        }

        private void onPickerStudentSelectedIndexChanged(object sender, EventArgs e)
        {
            selectedStudent = (Student) pickerStudent.SelectedItem;
        }

        private async void onBtnEditStudentClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(StudentEditor));
        }
    }
}