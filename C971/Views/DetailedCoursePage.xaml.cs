using C971.Model;
using C971.Services;

namespace C971.Views;

public partial class DetailedCoursePage : ContentPage
{
	private Course _course;
	public DetailedCoursePage(Course course)
	{
		InitializeComponent();
		_course = course;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCourseDetails();
    }

    private async Task LoadCourseDetails()
    {
		var updatedCourse = await CourseService.GetById(_course.Id);

        if (updatedCourse != null)
        {
            _course = updatedCourse;
            BindingContext = _course;
        }
    }

    private async void OnEditClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new EditCoursePage(_course));
	}

	private async void OnDeleteClicked(object sender, EventArgs e)
	{
		bool confirmDelete = await DisplayAlert(
			"Confirm Deletion",
			"Are you sure you want to delete this course?",
			"Yes",
			"No");

		if (confirmDelete)
		{
			await CourseService.RemoveCourse(_course.Id);

			await DisplayAlert("Success", "The course has been deleted", "OK");

			await Navigation.PopAsync();
		}
	}
}