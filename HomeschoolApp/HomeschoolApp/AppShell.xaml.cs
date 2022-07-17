using HomeschoolApp.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace HomeschoolApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            Routing.RegisterRoute(nameof(StudentEditor), typeof(StudentEditor));
            Routing.RegisterRoute(nameof(ActivitiesPage), typeof(ActivitiesPage));
            Routing.RegisterRoute(nameof(ActivityEditor), typeof(ActivityEditor));
            Routing.RegisterRoute(nameof(ActivityViewer), typeof(ActivityViewer));
        }

    }
}
