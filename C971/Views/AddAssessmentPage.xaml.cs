using C971.Model;
using C971.Services;

namespace C971.Views;

public partial class AddAssessmentPage : ContentPage
{
	private Course _course;
	private List<string> _assessmentTypes;
	public AddAssessmentPage(Course course)
	{
		InitializeComponent();
		_course = course;

		_assessmentTypes = new List<string> { "Objective", "Performance"};
		AssessmentTypePicker.ItemsSource = _assessmentTypes;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (_course.ObjectiveAssessment)
		{
			_assessmentTypes.Remove("Objective");
		}

        if (_course.PerformanceAssessment)
        {
            _assessmentTypes.Remove("Performance");
        }

		AssessmentTypePicker.ItemsSource = null;
        AssessmentTypePicker.ItemsSource = _assessmentTypes;

        if (_assessmentTypes.Count == 0) 
		{
			await DisplayAlert("Notice", "This course already has both assessment types.", "OK");
			await Navigation.PopAsync();
		}
    }

	private async void OnSaveClicked(object sender, EventArgs e)
	{

		//ADD VALIDATION HERE OR IN NEW METHOD


		var assessment = new Assessment
		{
			Name = NameEntry.Text,
			StartDate = StartDatePicker.Date,
			DueDate = DueDatePicker.Date,
			StartDateNotification = StartDateNotification.IsToggled,
			DueDateNotification = DueDateNotification.IsToggled,
			Type = (AssessmentType)AssessmentTypePicker.SelectedIndex,
			CourseId = _course.Id
		};

		await AssessmentService.AddAssessment(assessment, _course);

		await Navigation.PopAsync();
	}
}