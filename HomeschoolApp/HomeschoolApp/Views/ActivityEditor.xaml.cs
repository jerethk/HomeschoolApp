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

        private List<StudentName> allStudents;
        
        public ActivityEditor()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // create list of learning areas
            collectionViewLearningAreas.ItemsSource = new List<string> { LearningAreas.ENG, LearningAreas.MAT, LearningAreas.SCI, LearningAreas.HUM, LearningAreas.ART, LearningAreas.TEC, LearningAreas.HEA, LearningAreas.LAN }; ;

            // retrieve list of student names  
            string errorMessage = "";
            var studentList = DataAccess.QueryAllStudents(out errorMessage);
            allStudents = (from student in studentList
                        select new StudentName()
                        {
                            Id = student.Id,
                            Name = student.FirstName
                        }).ToList();
            collectionViewStudents.ItemsSource = allStudents;
            
            //label1.Text = $"{Mode} {ActivityId}";

            // Set up page
            if (Mode == "new")
            {
                pageHeading.Text = "New Activity";
                entryDuration.Text = "30";
            }
            else if (Mode == "edit")
            {
                pageHeading.Text = $"Edit Activity";

                int id = Int32.Parse(ActivityId);
                var activity = DataAccess.QueryActivityById(id, out errorMessage);
                var activityStudents = DataAccess.QueryActivityStudents(id, out errorMessage);

                if (activity != null)
                {
                    entryTitle.Text = activity.Title;
                    pickerDate.Date = DateTime.Parse(activity.Date);
                    pickerTimeStarted.Time = TimeSpan.Parse(activity.TimeStarted);
                    entryDuration.Text = activity.DurationMinutes.ToString();
                    entryLocation.Text = activity.Location;
                    editorDescription.Text = activity.Description;
                    editorNotes.Text = activity.Notes;
                    checkboxCompleted.IsChecked = activity.IsCompleted;

                    if (activity.LearningAreas.Contains("ENG")) collectionViewLearningAreas.SelectedItems.Add(LearningAreas.ENG);
                    if (activity.LearningAreas.Contains("MAT")) collectionViewLearningAreas.SelectedItems.Add(LearningAreas.MAT);
                    if (activity.LearningAreas.Contains("SCI")) collectionViewLearningAreas.SelectedItems.Add(LearningAreas.SCI);
                    if (activity.LearningAreas.Contains("HUM")) collectionViewLearningAreas.SelectedItems.Add(LearningAreas.HUM);
                    if (activity.LearningAreas.Contains("ART")) collectionViewLearningAreas.SelectedItems.Add(LearningAreas.ART);
                    if (activity.LearningAreas.Contains("TEC")) collectionViewLearningAreas.SelectedItems.Add(LearningAreas.TEC);
                    if (activity.LearningAreas.Contains("HEA")) collectionViewLearningAreas.SelectedItems.Add(LearningAreas.HEA);
                    if (activity.LearningAreas.Contains("LAN")) collectionViewLearningAreas.SelectedItems.Add(LearningAreas.LAN);


                    foreach (Student student in activityStudents)
                    {
                        foreach (StudentName pStudent in allStudents)
                        {
                            if (student.Id == pStudent.Id)
                            {
                                collectionViewStudents.SelectedItems.Add(pStudent);
                            }
                        }
                    }
                }
            }
        }


        private async void OnBtnSaveClicked(object sender, EventArgs e)
        {
            // validation 
            bool isValid = ValidateEntries();

            if (isValid)
            {
                bool isSaveSuccessful = true;
                
                Activity activity = new Activity();
                activity.Title = entryTitle.Text;
                activity.Date = pickerDate.Date.ToString();
                activity.TimeStarted = pickerTimeStarted.Time.ToString();
                activity.DurationMinutes = Int32.Parse(entryDuration.Text);
                activity.Location = entryLocation.Text;
                activity.IsCompleted = checkboxCompleted.IsChecked;
                activity.Description = editorDescription.Text;
                activity.Notes = editorNotes.Text;

                string learningAreasString = "";
                if (collectionViewLearningAreas.SelectedItems.Contains(LearningAreas.ENG)) learningAreasString += "ENG ";
                if (collectionViewLearningAreas.SelectedItems.Contains(LearningAreas.MAT)) learningAreasString += "MAT ";
                if (collectionViewLearningAreas.SelectedItems.Contains(LearningAreas.SCI)) learningAreasString += "SCI ";
                if (collectionViewLearningAreas.SelectedItems.Contains(LearningAreas.HUM)) learningAreasString += "HUM ";
                if (collectionViewLearningAreas.SelectedItems.Contains(LearningAreas.ART)) learningAreasString += "ART ";
                if (collectionViewLearningAreas.SelectedItems.Contains(LearningAreas.TEC)) learningAreasString += "TEC ";
                if (collectionViewLearningAreas.SelectedItems.Contains(LearningAreas.HEA)) learningAreasString += "HEA ";
                if (collectionViewLearningAreas.SelectedItems.Contains(LearningAreas.LAN)) learningAreasString += "LAN ";
                activity.LearningAreas = learningAreasString;

                string errorString = "";

                if (Mode == "new")
                {
                    // Add activity to activities table
                    int newActivityId = DataAccess.AddNewActivity(activity, out errorString);

                    if (newActivityId == -1)
                    {
                        isSaveSuccessful = false;
                    }
                    else
                    {
                        // Insert records to Activities-Students table
                        foreach (StudentName student in collectionViewStudents.SelectedItems)
                        {
                            ActivityStudent activityStudent = new ActivityStudent();
                            activityStudent.Activity = newActivityId;
                            activityStudent.Student = student.Id;
                            isSaveSuccessful = DataAccess.AddActivityStudent(activityStudent, out errorString);
                        }
                    }
                }
                else if (Mode == "edit")
                {
                    // Update activity in activities table
                    activity.Id = Int32.Parse(ActivityId);
                    isSaveSuccessful = DataAccess.UpdateActivity(activity, out errorString);

                    // Delete previous records in activities_students table
                    isSaveSuccessful = DataAccess.DeleteActivityStudent(activity.Id, out errorString);

                    // Insert new records to activities_students table
                    foreach (StudentName student in collectionViewStudents.SelectedItems)
                    {
                        ActivityStudent activityStudent = new ActivityStudent();
                        activityStudent.Activity = activity.Id;
                        activityStudent.Student = student.Id;
                        isSaveSuccessful = DataAccess.AddActivityStudent(activityStudent, out errorString);
                    }
                }

                if (isSaveSuccessful)
                {
                    await DisplayAlert("", "Activity saved", "ok");

                    string parameters = (Mode == "edit") ? $"?activityid={activity.Id}" : "";
                    await Shell.Current.GoToAsync(".." + parameters);
                }
                else
                {
                    await DisplayAlert("", "Failed to save activity", "ok");
                }
            }
        }

        // Method to validate fields, returns false if there is an invalid entry
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