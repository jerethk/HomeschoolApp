using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeschoolApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivitiesPage : ContentPage
    {
        public ActivitiesPage()
        {
            InitializeComponent();
        }

        private async void onBtnAddActivityClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ActivityEditor) + "?mode=new&id=-1");
        }
    }
}