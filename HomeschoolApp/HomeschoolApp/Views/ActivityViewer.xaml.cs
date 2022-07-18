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
    [QueryProperty("ActivityId", "activityid")]
    public partial class ActivityViewer : ContentPage
    {
        private Activity activity;

        public string ActivityId { get; set; }

        public ActivityViewer()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            string errorString = "";
            int id = Int32.Parse(ActivityId);
            activity = DataAccess.QueryActivityById(id, out errorString);

            if (activity != null)
            {
                labelTitle.Text = activity.Title;
                labelDate.Text = DateTime.Parse(activity.Date).Date.ToString();
                labelTimeStarted.Text = $"Time started: {activity.TimeStarted}";
                labelDuration.Text = $"Duration: {activity.DurationMinutes} min";
                labelLocation.Text = $"Location: {activity.Location}";
                labelDescription.Text = $"Description: {activity.Description}";
                labelNotes.Text = $"Notes: {activity.Notes}";

                string learningAreasString = "Learning areas:\n";
                if (activity.LearningAreas.Contains("ENG")) learningAreasString += $"* {LearningAreas.ENG}\n";
                if (activity.LearningAreas.Contains("MAT")) learningAreasString += $"* {LearningAreas.MAT}\n";
                if (activity.LearningAreas.Contains("SCI")) learningAreasString += $"* {LearningAreas.SCI}\n";
                if (activity.LearningAreas.Contains("HUM")) learningAreasString += $"* {LearningAreas.HUM}\n";
                if (activity.LearningAreas.Contains("ART")) learningAreasString += $"* {LearningAreas.ART}\n";
                if (activity.LearningAreas.Contains("TEC")) learningAreasString += $"* {LearningAreas.TEC}\n";
                if (activity.LearningAreas.Contains("HEA")) learningAreasString += $"* {LearningAreas.HEA}\n";
                if (activity.LearningAreas.Contains("LAN")) learningAreasString += $"* {LearningAreas.LAN}\n";
                labelLearningAreas.Text = learningAreasString;

                string studentsString = "Students:\n";
                var students = DataAccess.QueryActivityStudents(activity.Id, out errorString);
                foreach (Student student in students)
                {
                    studentsString += $"* {student.FirstName}\n";
                }

                labelStudents.Text = studentsString;
            }
        }

        private async void OnBtnEditActivityClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ActivityEditor) + $"?mode=edit&id={ActivityId}");
        }
    }
}