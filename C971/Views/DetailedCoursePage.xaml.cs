using C971.Model;
using C971.Services;
using Plugin.LocalNotification;


namespace C971.Views;

public partial class DetailedCoursePage : ContentPage
{
	private Course _course;
	private Assessment _assessment;
	private bool _isPageLoaded;
	public DetailedCoursePage(Course course)
	{
		InitializeComponent();
		_course = course;
		_assessment = new Assessment();
		
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCourseDetails();
		UpdateAssessmentVisibility();
		LoadAssessments();
        _isPageLoaded = true;
    }

	private async void LoadAssessments()
	{
		if(_course.PerformanceAssessment == true)
		{
			_assessment = await AssessmentService.GetAssessmentByType(_course.Id, AssessmentType.Performance);

			PerformanceName.Text = _assessment.Name;
			PerformanceDueDate.Text = $"Due: {_assessment.DueDate.ToString()}";
		}

		if (_course.ObjectiveAssessment == true)
		{
			_assessment = await AssessmentService.GetAssessmentByType(_course.Id, AssessmentType.Objective);

			ObjectiveName.Text = _assessment.Name;
			ObjectiveDueDate.Text = $"Due: {_assessment.DueDate.ToString()}";
		}
	}

	private async void OnAddPerformanceAssessmentClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new AddAssessmentPage(_course));
	}

    private async void OnAddObjectiveAssessmentClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddAssessmentPage(_course));
    }

    private void UpdateAssessmentVisibility()
	{

		if(_course.PerformanceAssessment == true)
		{
			PerformanceInformation.IsVisible = true;
			AddPerformanceAssessmentButton.IsVisible = false;
		}
		else
		{
            PerformanceInformation.IsVisible = false;
            AddPerformanceAssessmentButton.IsVisible = true;
        }

        if (_course.ObjectiveAssessment == true)
        {
            ObjectiveInformation.IsVisible = true;
            AddObjectiveAssessmentButton.IsVisible = false;
        }
        else
        {
            ObjectiveInformation.IsVisible = false;
            AddObjectiveAssessmentButton.IsVisible = true;
        }

		
	}

	private async Task LoadCourseDetails()
    {
		var updatedCourse = await CourseService.GetById(_course.Id);

        if (updatedCourse != null)
        {
            _course = updatedCourse;
            BindingContext = _course;

			if(_course.Notes != null)
			{
				AddNotesButton.IsVisible = false;
				NotesLabel.IsVisible = true;
				NotesButtonGrid.IsVisible = true;
			}
			else
			{
				AddNotesButton.IsVisible = true;
                NotesLabel.IsVisible = false;
                NotesButtonGrid.IsVisible = false;
            }
        }
    }

	private async void OnStartNotificationToggled(object sender, ToggledEventArgs e)
	{

		if (!_isPageLoaded) return;

		var isNotificationEnabled = e.Value;
		int startNotificationId = _course.TermId * 10000 + _course.Id * 10 + 1;
		
		_course.StartDateNotification = isNotificationEnabled;

		await CourseService.UpdateCourse(_course);

		if (isNotificationEnabled)
		{
			var notification = new NotificationRequest()
			{
				NotificationId = startNotificationId,
				Title = "Course Start Reminder",
				Description = $"Your course {_course.Name} starts today!",
				Schedule = new NotificationRequestSchedule
				{
					NotifyTime = _course.StartDate,
				}
			};

			await LocalNotificationCenter.Current.Show(notification);
			await DisplayAlert("Notification On", $"Start date notification for this course turned on. You will be notified {_course.StartDate:MMMM dd, yyyy}.", "OK");
		}
		else
		{
			LocalNotificationCenter.Current.Clear(startNotificationId);
            await DisplayAlert("Notification Off", $"Start date notification for this course turned off. You will no longer be notified.", "OK");
        }
		
	}

    private async void OnEndNotificationToggled(object sender, ToggledEventArgs e)
    {
        if (!_isPageLoaded) return;
        var isNotificationEnabled = e.Value;
        int endNotificationId = _course.TermId * 10000 + _course.Id * 10 + 1;

        _course.EndDateNotification = isNotificationEnabled;

        await CourseService.UpdateCourse(_course);

        if (isNotificationEnabled)
        {
            var notification = new NotificationRequest()
            {
                NotificationId = endNotificationId,
                Title = "Course End Reminder",
                Description = $"Your course {_course.Name} ends today!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = _course.EndDate,
                }
            };

            await LocalNotificationCenter.Current.Show(notification);
            await DisplayAlert("Notification On", $"End date notification for this course turned on. You will be notified {_course.EndDate:MMMM dd, yyyy}.", "OK");
        }
        else
        {
            LocalNotificationCenter.Current.Clear(endNotificationId);
            await DisplayAlert("Notification Off", $"End date notification for this course turned off. You will no longer be notified.", "OK");
        }

    }

    public async Task ShareText(string text)
    {
        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = text,
            Title = "Share Note"
        });
    }

    private async void OnShareNotesClicked(object sender, EventArgs e)
	{
		await ShareText(NotesLabel.Text);
	}

	private async void OnDeleteNotesClicked(object sender, EventArgs e)
	{
		bool confirmDelete = await DisplayAlert(
			"Confirm Deletion",
			"Are you sure you want to delete this note?",
			"Yes",
			"No");

		if (confirmDelete)
		{
			_course.Notes = null;
			await CourseService.UpdateCourse(_course);
            await DisplayAlert("Success", "The note has been deleted", "OK");
            OnAppearing();
		}
	}


	private async void OnAddNotesClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new NotesPage(_course));
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