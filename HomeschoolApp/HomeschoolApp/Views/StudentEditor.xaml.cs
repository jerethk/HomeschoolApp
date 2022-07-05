using System;
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
    public partial class StudentEditor : ContentPage
    {
        private List<Student> studentList;
        private Student selectedStudent;

        public StudentEditor()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            string errorString;
            studentList = DataAccess.queryAllStudents(out errorString);

            if (studentList != null)
            {
                pickerStudent.ItemsSource = studentList;
                pickerStudent.ItemDisplayBinding = new Binding("FirstName");
                pickerStudent.SelectedIndex = 0;

                CheckBoxNewStudent.IsChecked = false;
                pickerStudent.IsEnabled = true;
            }
        }

        private void OnCheckBoxNewStudentChanged(object sender, CheckedChangedEventArgs e)
        {
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

        private void OnBtnSaveClicked(object sender, EventArgs e)
        {

        }

        private void UpdateFields()
        {
            if (pickerStudent.SelectedIndex >= 0)
            {
                selectedStudent = (Student)pickerStudent.SelectedItem;

                // Fill entries with student data
                entryFirstName.Text = selectedStudent.FirstName;
                entryLastName.Text = selectedStudent.LastName;
                pickerDob.Date = selectedStudent.Dob;
                pickerSex.SelectedIndex = (selectedStudent.Sex == Sex.M) ? 0 : 1;
                pickerYearLevel.SelectedIndex = selectedStudent.YearLevel;
                //studentImage.
                editorNotes.Text = selectedStudent.Notes;
            }
        }

    }
}