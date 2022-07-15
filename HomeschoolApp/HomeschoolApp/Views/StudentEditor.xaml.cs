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
            studentList = DataAccess.queryAllStudents(out errorString);

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

        private async void OnBtnSaveClicked(object sender, EventArgs e)
        {
            string errorString = "";

            if (CheckBoxNewStudent.IsChecked)
            {
                // Add new student
                // validation

                Student newStudent = new Student();
                newStudent.FirstName = entryFirstName.Text;
                newStudent.LastName = entryLastName.Text;
                newStudent.Dob = pickerDob.Date;
                newStudent.Sex = (pickerSex.SelectedIndex == 0) ? Sex.M : Sex.F;
                newStudent.YearLevel = pickerYearLevel.SelectedIndex;
                newStudent.Notes = editorNotes.Text;
                DataAccess.AddNewStudent(newStudent, out errorString);
            }
            else
            {
                // Update student details
                // validation

                Student updatedStudent = new Student();
                updatedStudent.Id = selectedStudent.Id;
                updatedStudent.FirstName = entryFirstName.Text;
                updatedStudent.LastName = entryLastName.Text;
                updatedStudent.Dob = pickerDob.Date;
                updatedStudent.Sex = (pickerSex.SelectedIndex == 0) ? Sex.M : Sex.F;
                updatedStudent.YearLevel = pickerYearLevel.SelectedIndex;
                updatedStudent.Notes = editorNotes.Text;
                
                DataAccess.UpdateStudent(updatedStudent, out errorString);
            }

            label1.Text = errorString; 
            await DisplayAlert("", "Done", "Ok");
            await Shell.Current.GoToAsync("..");
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