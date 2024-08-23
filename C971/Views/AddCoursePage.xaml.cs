using C971.Model;
using C971.Services;

namespace C971.Views;

public partial class AddCoursePage : ContentPage
{
	Course _course = new Course();
    Term _term;
	public AddCoursePage(Term term)
	{
		InitializeComponent();
        _term = term;
		/*CourseNameEntry.Text = _course.Name;
		StartDatePicker.Date = _course.StartDate;
		EndDatePicker.Date = _course.EndDate;
		CourseStatusPicker.SelectedItem = _course.Status;
		InstructorNameEntry.Text = _course.InstructorName;
		InstructorPhoneEntry.Text = _course.InstructorPhone;
		InstructorEmailEntry.Text = _course.InstructorEmail;
		NotesEditor.Text = _course.Notes;
		StartDateNotifSwitch.IsToggled = _course.StartDateNotification;
		EndDateNotifSwitch.IsToggled = _course.EndDateNotification;*/
	}


    private async void OnSaveCourseClicked(object sender, EventArgs e)
    {
        _course.Name = CourseNameEntry.Text;
        _course.StartDate = StartDatePicker.Date;
        _course.EndDate = EndDatePicker.Date;
        _course.Status = CourseStatusPicker.SelectedItem?.ToString();
        _course.InstructorName = InstructorNameEntry.Text;
        _course.InstructorPhone = InstructorPhoneEntry.Text;
        _course.InstructorEmail = InstructorEmailEntry.Text;
        _course.Notes = NotesEditor.Text;
        _course.TermId = _term.Id;
        _course.StartDateNotification = StartDateNotifSwitch.IsToggled;
        _course.EndDateNotification = EndDateNotifSwitch.IsToggled;

        await CourseService.AddCourse(_course);

        await Navigation.PopAsync();
    }
}