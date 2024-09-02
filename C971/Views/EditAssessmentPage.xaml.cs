using C971.Model;
using C971.Services;

namespace C971.Views;

public partial class EditAssessmentPage : ContentPage
{
	Assessment _assessment;
	public EditAssessmentPage(Assessment assessment)
	{
		InitializeComponent();
		_assessment = assessment;
		NameEntry.Text = assessment.Name;
		StartDatePicker.Date = assessment.StartDate;
		DueDatePicker.Date = assessment.DueDate;
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

        return true;
	}

	private async void OnSaveClicked(object sender, EventArgs e)
	{
		if (!ValidateFields()) return;

		_assessment.Name = NameEntry.Text;
		_assessment.StartDate = StartDatePicker.Date;
		_assessment.DueDate = DueDatePicker.Date;

		await AssessmentService.UpdateAssessment(_assessment);
		await DisplayAlert("Success", "Successfully updated assessment", "OK");
		await Navigation.PopAsync();
	}
}