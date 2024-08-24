using C971.Model;
using C971.Services;
using System.Text.RegularExpressions;

namespace C971.Views;

public partial class EditCoursePage : ContentPage
{
	private Course _course;
	public EditCoursePage(Course course)
	{
		InitializeComponent();
		_course = course;
		
		CourseNameEntry.Text = _course.Name;
		StartDatePicker.Date = _course.StartDate;
		EndDatePicker.Date = _course.EndDate;
		CourseStatusPicker.SelectedItem = _course.Status;
		InstructorNameEntry.Text = _course.InstructorName;
		InstructorPhoneEntry.Text = _course.InstructorPhone;
		InstructorEmailEntry.Text = _course.InstructorEmail;
	}

	private bool ValidateCourse()
	{
		if (string.IsNullOrWhiteSpace(CourseNameEntry.Text))
		{
            DisplayAlert("Error", "Course name cannot be empty", "OK");
            return false;
        }

        if (EndDatePicker.Date <= StartDatePicker.Date)
        {
            DisplayAlert("Error", "End date must be after the start date", "OK");
            return false;
        }

        if (CourseStatusPicker.SelectedItem == null)
        {
            DisplayAlert("Error", "Please select a course status", "OK");
            return false;
        }

        if (string.IsNullOrWhiteSpace(InstructorNameEntry.Text))
        {
            DisplayAlert("Error", "Instructor name cannot be empty", "OK");
            return false;
        }

		if(string.IsNullOrWhiteSpace(InstructorPhoneEntry.Text) || !IsValidPhoneNumber(InstructorPhoneEntry.Text))
		{
            DisplayAlert("Error", "Please enter a valid phone number", "OK");
            return false;
        }

        if (string.IsNullOrWhiteSpace(InstructorEmailEntry.Text) || !IsValidEmail(InstructorEmailEntry.Text))
        {
            DisplayAlert("Error", "Please enter a valid email", "OK");
            return false;
        }

       

        return true;
	}

	private bool IsValidPhoneNumber(string phoneNumber)
	{
		var phoneRegex = @"^\+?\d{10,15}$";
		return Regex.IsMatch(phoneNumber, phoneRegex);
	}

	private bool IsValidEmail(string email)
	{
		var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
		return Regex.IsMatch(email, emailRegex);
	}

	private async void OnSaveCourseClicked(object sender, EventArgs e)
	{
		if (!ValidateCourse())
		{
			return;
		}

		_course.Name = CourseNameEntry.Text;
        _course.StartDate = StartDatePicker.Date;
		_course.EndDate = EndDatePicker.Date;
		_course.Status = CourseStatusPicker.SelectedItem?.ToString();
		_course.InstructorName = InstructorNameEntry.Text;
		_course.InstructorPhone = InstructorPhoneEntry.Text;
		_course.InstructorEmail = InstructorEmailEntry.Text;


        await CourseService.UpdateCourse(_course);

		await DisplayAlert("Success", "Course updated successfully", "OK");
		await Navigation.PopAsync();
	}
}