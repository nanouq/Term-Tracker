using C971.Model;
using C971.Services;
using System.Collections.ObjectModel;
using C971.Views;
using System.Diagnostics;

namespace C971
{
    public partial class MainPage : ContentPage
    {
        private bool CreatedTermForEvaluation = false;
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnTermSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Term selectedTerm)
            {
                await Navigation.PushAsync(new DetailedTermPage(selectedTerm));

                ((CollectionView)sender).SelectedItem = null;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await SeedData();
            var terms = await TermService.GetTerm();
            TermCollectionView.ItemsSource = terms;
            if (terms.Any())
            {
                NoTermsAdded.IsVisible = false;
            }
            else
            {
                NoTermsAdded.IsVisible = true;
            }
        }

        private async void OnAddTermClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTermPage());
        }

        //This method will load a sample term and course when the app is loaded just for evaluation purposes.
        //The term can be deleted within the same session but if the app restarts it will add again.
        public async Task SeedData()
        {
            var existingTerm = await TermService.GetTermByName("Fall 2024");
            if (existingTerm != null)
            {
                return;
            }

            if(CreatedTermForEvaluation == true)
            {
                return;
            }

            //Create Term For Evaluation Purposes
            string termName = "Fall 2024";
            DateTime termStart = new DateTime(2024, 8, 26);
            DateTime termEnd = new DateTime(2024, 12, 9);

            await TermService.AddTerm(termName, termStart, termEnd);
            var newTerm = await TermService.GetTermByName(termName);

            //Create Course For Evaluation Purposes
            var course = new Course
            {
                Name = "C971",
                StartDate = new DateTime(2024, 9, 1),
                EndDate = new DateTime(2024, 10, 1),
                Status = "In Progress",
                InstructorName = "Anika Patel",
                InstructorPhone = "555-123-4567",
                InstructorEmail = "anika.patel@strimeuniversity.edu",
                TermId = newTerm.Id,
                Notes = "This is a sample note",
                PerformanceAssessment = true,
                ObjectiveAssessment = true
            };

            await CourseService.AddCourse(course);
            var newCourse = await CourseService.GetCourseByName(course.Name);

            //Create Performance Assessment For Evaluation Purposes
            var pAssessment = new Assessment
            {
                Name = "Sample Performance Assessment",
                StartDate = new DateTime(2024,9,1),
                DueDate = new DateTime(2024,9,15),
                Type = AssessmentType.Performance,
                CourseId = newCourse.Id
            };

            await AssessmentService.AddAssessmentWithoutCourse(pAssessment);

            var oAssessment = new Assessment
            {
                Name = "Sample Objective Assessment",
                StartDate = new DateTime(2024, 9, 16),
                DueDate = new DateTime(2024, 10, 1),
                Type = AssessmentType.Objective,
                CourseId = newCourse.Id
            };

            await AssessmentService.AddAssessmentWithoutCourse(oAssessment);
            CreatedTermForEvaluation = true;
        }
    }
}
