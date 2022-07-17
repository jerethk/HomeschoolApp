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
                labelDate.Text = activity.Date;
                labelTimeStarted.Text = activity.TimeStarted;
                labelDuration.Text = activity.DurationMinutes.ToString();
                labelLocation.Text = activity.Location;
                labelLearningAreas.Text = activity.LearningAreas;
            }
        }
    }
}