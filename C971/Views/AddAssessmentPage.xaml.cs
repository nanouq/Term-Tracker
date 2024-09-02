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

	private bool ValidateFields()
	{
		if (string.IsNullOrWhiteSpace(NameEntry.Text))
		{
            DisplayAlert("Error", "Assessment name cannot be empty", "OK");
            return false;
        }

        if (DueDatePicker.Date <= StartDatePicker.Date)
        {
            DisplayAlert("Error", "Due date must be after the start date", "OK");
            return false;
        }

		if (AssessmentTypePicker.SelectedIndex == -1)
		{
            DisplayAlert("Error", "Please select an assessment type", "OK");
            return false;
        }
        return true;
	}

	private async void OnSaveClicked(object sender, EventArgs e)
	{
		if (!ValidateFields()) return;

		var selectedTypeText = AssessmentTypePicker.SelectedItem.ToString();
		AssessmentType selectedType = selectedTypeText == "Objective" ? AssessmentType.Objective : AssessmentType.Performance;

		var assessment = new Assessment
		{
			Name = NameEntry.Text,
			StartDate = StartDatePicker.Date,
			DueDate = DueDatePicker.Date,
			Type = selectedType,
			CourseId = _course.Id
		};

		await AssessmentService.AddAssessment(assessment, _course);
        await DisplayAlert("Success", "Successfully added assessment", "OK");
        await Navigation.PopAsync();
	}
}