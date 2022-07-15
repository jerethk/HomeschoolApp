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
            collectionViewLearningAreas.ItemsSource = new List<string> { "English", "Maths", "Science", "Humanities & Social Sciences", "Arts", "Tech", "Health & PE", "Languages" }; ;

            // retrieve list of student names for listview
            string errorMessage = "";
            var studentList = DataAccess.queryAllStudents(out errorMessage);
            students = (from student in studentList
                        select new StudentName()
                        {
                            Id = student.Id,
                            Name = student.FirstName
                        }).ToList();
            collectionViewStudents.ItemsSource = students;
            
            label1.Text = $"{Mode} {ActivityId}";
        }

        private void clickmeed(object sender, EventArgs e)
        {
            label1.Text = $"{entryDuration.Height} {collectionViewLearningAreas.Height}";
        }
    }
}