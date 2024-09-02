using C971.Model;
using C971.Services;
using Plugin.LocalNotification;

namespace C971.Views;

public partial class DetailedAssessmentPage : ContentPage
{
	private Assessment _assessment;
    private bool _isPageLoaded;
    int startNotificationId;
    int dueNotificationId;
    DateTime oldStartDate;
    DateTime oldDueDate;
    public DetailedAssessmentPage(Assessment assessment)
	{
		InitializeComponent();
		_assessment = assessment;
        oldStartDate = assessment.StartDate;
        oldDueDate = assessment.DueDate;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAssessmentDetails();
        UpdateNotifications();
        _isPageLoaded = true;
    }

    private async Task LoadAssessmentDetails()
    {
        var updatedAssessment = await AssessmentService.GetAssessmentByType(_assessment.CourseId, _assessment.Type);

        if (updatedAssessment != null)
        {
            _assessment = updatedAssessment;
            BindingContext = _assessment;
            startLabel.Text = $"{_assessment.StartDate:MMMM dd, yyyy}";
            dueLabel.Text = $"{_assessment.DueDate:MMMM dd, yyyy}";
            dueDateMain.Text = $"{_assessment.DueDate:dddd, MMMM dd, yyyy}";
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
	{
        bool confirmDelete = await DisplayAlert(
            "Confirm Deletion",
            "Are you sure you want to delete this assessment?",
            "Yes",
            "No");

        if (confirmDelete)
        {
            await AssessmentService.RemoveAssessment(_assessment);

            await DisplayAlert("Success", "The assessment has been deleted", "OK");
            LocalNotificationCenter.Current.Clear(startNotificationId);
            LocalNotificationCenter.Current.Clear(dueNotificationId);

            await Navigation.PopAsync();
        }
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditAssessmentPage(_assessment));
    }

    private async void UpdateNotifications()
    {

            if (_assessment.StartDate != oldStartDate)
            {
                LocalNotificationCenter.Current.Clear(startNotificationId);
                var notification = new NotificationRequest()
                {
                    NotificationId = startNotificationId,
                    Title = "Assessment Start Reminder",
                    Description = $"Your assessment {_assessment.Name} starts today!",
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = _assessment.StartDate,
                    }
                };

                await LocalNotificationCenter.Current.Show(notification);
                await DisplayAlert("Notification Update", $"Start date notification for this assessment was updated. You will be notified {_assessment.StartDate:MMMM dd, yyyy}.", "OK");
            }

            if (_assessment.DueDate != oldDueDate)
            {
                LocalNotificationCenter.Current.Clear(dueNotificationId);
                var notification = new NotificationRequest()
                {
                    NotificationId = dueNotificationId,
                    Title = "Assessment Due Reminder",
                    Description = $"Your assessment {_assessment.Name} is due today!",
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = _assessment.DueDate,
                    }
                };

                await LocalNotificationCenter.Current.Show(notification);
                await DisplayAlert("Notification Update", $"Due date notification for this assessment was updated. You will be notified {_assessment.DueDate:MMMM dd, yyyy}.", "OK");
            }
        
    }

    private async void OnStartNotificationToggled(object sender, ToggledEventArgs e)
    {
        if (!_isPageLoaded) return;

        var isNotificationEnabled = e.Value;
        startNotificationId = _assessment.CourseId * 20000 + _assessment.Id * 20 + 2;

        _assessment.StartDateNotification = isNotificationEnabled;

        await AssessmentService.UpdateAssessment(_assessment);

        if (isNotificationEnabled)
        {
            var notification = new NotificationRequest()
            {
                NotificationId = startNotificationId,
                Title = "Assessment Start Reminder",
                Description = $"Your assessment {_assessment.Name} starts today!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = _assessment.StartDate,
                }
            };

            await LocalNotificationCenter.Current.Show(notification);
            await DisplayAlert("Notification On", $"Start date notification for this assessment turned on. You will be notified {_assessment.StartDate:MMMM dd, yyyy}.", "OK");
        }
        else
        {
            LocalNotificationCenter.Current.Clear(startNotificationId);
            await DisplayAlert("Notification Off", $"Start date notification for this assessment turned off. You will no longer be notified.", "OK");
        }

    }

    private async void OnDueNotificationToggled(object sender, ToggledEventArgs e)
    {
        if (!_isPageLoaded) return;

        var isNotificationEnabled = e.Value;
        dueNotificationId = _assessment.CourseId * 20000 + _assessment.Id * 20 + 2;

        _assessment.DueDateNotification = isNotificationEnabled;

        await AssessmentService.UpdateAssessment(_assessment);

        if (isNotificationEnabled)
        {
            var notification = new NotificationRequest()
            {
                NotificationId = dueNotificationId,
                Title = "Assessment Due Reminder",
                Description = $"Your assessment {_assessment.Name} is due today!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = _assessment.DueDate,
                }
            };

            await LocalNotificationCenter.Current.Show(notification);
            await DisplayAlert("Notification On", $"Due date notification for this assessment turned on. You will be notified {_assessment.DueDate:MMMM dd, yyyy}.", "OK");
        }
        else
        {
            LocalNotificationCenter.Current.Clear(dueNotificationId);
            await DisplayAlert("Notification Off", $"Due date notification for this assessment turned off. You will no longer be notified.", "OK");
        }
    }
}

