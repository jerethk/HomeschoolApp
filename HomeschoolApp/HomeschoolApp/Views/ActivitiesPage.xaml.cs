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
    public partial class ActivitiesPage : ContentPage
    {
        private List<Activity> activityList;
        
        public ActivitiesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            string errorString = "";
            activityList = DataAccess.QueryAllActivities(out errorString);

            if (activityList != null)
            {
                collectionViewActivities.ItemsSource = activityList;
            }
        }

        private async void onBtnAddActivityClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ActivityEditor) + "?mode=new&id=-1");
        }

        private async void onActivitySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (collectionViewActivities.SelectedItem != null)
            {
                Activity selectedActivity = (Activity) collectionViewActivities.SelectedItem;
                await Shell.Current.GoToAsync(nameof(ActivityViewer) + $"?activityid={selectedActivity.Id}");
            }
        }
    }
}