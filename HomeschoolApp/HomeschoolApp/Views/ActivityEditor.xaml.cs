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
    [QueryProperty("Mode", "mode")]
    [QueryProperty("ActivityId", "id")]
    public partial class ActivityEditor : ContentPage
    {
        public string Mode { get; set; }
        public string ActivityId { get; set; }

        private List<StudentName> students;
        
        public ActivityEditor()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // create list of learning areas
            collectionViewLearningAreas.ItemsSource = new List<string> { LearningAreas.ENG, LearningAreas.MAT, LearningAreas.SCI, LearningAreas.HUM, LearningAreas.ART, LearningAreas.TEC, LearningAreas.HEA, LearningAreas.LAN }; ;

            // retrieve list of student names for listview
            string errorMessage = "";
            var studentList = DataAccess.QueryAllStudents(out errorMessage);
            students = (from student in studentList
                        select new StudentName()
                        {
                            Id = student.Id,
                            Name = student.FirstName
                        }).ToList();
            collectionViewStudents.ItemsSource = students;
            
            label1.Text = $"{Mode} {ActivityId}";

            //
            if (Mode == "new")
            {
                pageHeading.Text = "New Activity";
                entryDuration.Text = "30";
            }
            else if (Mode == "edit")
            {
                pageHeading.Text = $"Edit Activity {ActivityId}";
            }
        }

        private void clickme(object sender, EventArgs e)
        {
            label1.Text = $"{entryDuration.Height} {collectionViewLearningAreas.Height}";
        }

        private async void OnBtnSaveClicked(object sender, EventArgs e)
        {
            // validation 
            bool isValid = ValidateEntries();

            if (isValid)
            {
                if (Mode == "new")
                { 
                    // Save activity to activities table
                    Activity newActivity = new Activity();
                    newActivity.Title = entryTitle.Text;
                    newActivity.Date = pickerDate.Date.ToString();
                    newActivity.TimeStarted = pickerTimeStarted.Time.ToString();
                    newActivity.DurationMinutes = Int32.Parse(entryDuration.Text);
                    newActivity.Location = entryLocation.Text;
                    newActivity.IsCompleted = checkboxCompleted.IsChecked;
                    newActivity.Description = editorDescription.Text;
                    newActivity.Notes = editorNotes.Text;

                    string learningAreasString = "";
                    if (collectionViewLearningAreas.SelectedItems.Contains(LearningAreas.ENG)) learningAreasString += "ENG ";
                    if (collectionViewLearningAreas.SelectedItems.Contains(LearningAreas.MAT)) learningAreasString += "MAT ";
                    if (collectionViewLearningAreas.SelectedItems.Contains(LearningAreas.SCI)) learningAreasString += "SCI ";
                    if (collectionViewLearningAreas.SelectedItems.Contains(LearningAreas.HUM)) learningAreasString += "HUM ";
                    if (collectionViewLearningAreas.SelectedItems.Contains(LearningAreas.ART)) learningAreasString += "ART ";
                    if (collectionViewLearningAreas.SelectedItems.Contains(LearningAreas.TEC)) learningAreasString += "TEC ";
                    if (collectionViewLearningAreas.SelectedItems.Contains(LearningAreas.HEA)) learningAreasString += "HEA ";
                    if (collectionViewLearningAreas.SelectedItems.Contains(LearningAreas.LAN)) learningAreasString += "LAN ";
                    newActivity.LearningAreas = learningAreasString;

                    string errorString = "";
                    int activityId = DataAccess.AddNewActivity(newActivity, out errorString);

                    // Save records to Activities-Students table
                    foreach (StudentName student in collectionViewStudents.SelectedItems)
                    {
                        ActivityStudent activityStudent = new ActivityStudent();
                        activityStudent.Activity = activityId;
                        activityStudent.Student = student.Id;
                        DataAccess.AddActivityStudent(activityStudent, out errorString);
                    }

                    await DisplayAlert("", "Activity saved", "ok");
                    await Shell.Current.GoToAsync("..");
                }
            }
            else if (Mode == "edit")
            {

            }

        }

        private bool ValidateEntries()
        {
            if (entryTitle.Text == "" || entryTitle.Text == null)
            {
                DisplayAlert("", "Title cannot be empty", "ok");
                return false;
            }

            if (pickerDate.Date > DateTime.Now)
            {
                DisplayAlert("", "Date cannot be future", "ok");
                return false;
            }

            int duration; 
            bool canParse = Int32.TryParse(entryDuration.Text, out duration);
            if (!canParse)
            {
                DisplayAlert("", "Not a valid duration", "ok");
                return false;
            }

            if (duration <= 0)
            {
                DisplayAlert("", "Duration must be greater than 0 minutes", "ok");
                return false;
            }

            if (collectionViewLearningAreas.SelectedItems.Count == 0)
            {
                DisplayAlert("", "You must choose at least one learning area", "ok");
                return false;
            }

            if (collectionViewStudents.SelectedItems.Count == 0)
            {
                DisplayAlert("", "You must choose at least one student", "ok");
                return false;
            }

            return true;
        }
    }
}