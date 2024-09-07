using C971.Model;
using C971.Services;

namespace C971.Views;

public partial class NotesPage : ContentPage
{
	private Course _course;
	public NotesPage(Course course)
	{
		InitializeComponent();
		_course = course;

		if (_course.Notes != null)
		{
			AddEditLabel.Text = "Edit Note:";
			NotesEditor.Text = _course.Notes;
		}

	}


	private async void OnSaveClicked(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(NotesEditor.Text))
		{
			await DisplayAlert("Error", "Please enter a valid note.", "OK");
			return;
		}

		_course.Notes = NotesEditor.Text;
		await CourseService.UpdateCourse(_course);
		if(AddEditLabel.Text == "Edit Note:")
		{
            await DisplayAlert("Success", "Successfully edited note.", "OK");
        }
		else
		{
            await DisplayAlert("Success", "Successfully added note.", "OK");
        }  
		await Navigation.PopAsync();
    }
}