using C971.Model;
using C971.Services;
using Plugin.LocalNotification;

namespace C971.Views;

public partial class DetailedAssessmentPage : ContentPage
{
	private Assessment _assessment;
    private bool _isPageLoaded;
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
        UpdateNotificationVisbility();
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

    private void UpdateNotifications()
    {
        if (_assessment.StartDateNotificationId != 0)
        {
            StartDateNotificationSwitch.IsToggled = true;
        }

        if (_assessment.DueDateNotificationId != 0)
        {
            DueDateNotificationSwitch.IsToggled = true;
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
            if(_assessment.StartDateNotificationId != 0)
            {
                LocalNotificationCenter.Current.Clear(_assessment.StartDateNotificationId);
            }

            if (_assessment.DueDateNotificationId != 0)
            {
                LocalNotificationCenter.Current.Clear(_assessment.DueDateNotificationId);
            }

            await AssessmentService.RemoveAssessment(_assessment);
            await DisplayAlert("Success", "The assessment has been deleted", "OK");
            await Navigation.PopAsync();
        }
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditAssessmentPage(_assessment));
    }

    private async void UpdateNotificationVisbility()
    {

        if (_assessment.StartDate != oldStartDate)
        {
            if (_assessment.StartDateNotificationId != 0)
            {
                LocalNotificationCenter.Current.Clear(_assessment.StartDateNotificationId);
                var notification = new NotificationRequest()
                {
                    NotificationId = _assessment.StartDateNotificationId,
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
        }

        if (_assessment.DueDate != oldDueDate)
        {

            if (_assessment.DueDateNotificationId != 0)
            {
                LocalNotificationCenter.Current.Clear(_assessment.DueDateNotificationId);
                var notification = new NotificationRequest()
                {
                    NotificationId = _assessment.DueDateNotificationId,
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
    }

    private async void OnStartNotificationToggled(object sender, ToggledEventArgs e)
    {
        if (!_isPageLoaded) return;

        var isNotificationEnabled = e.Value;

        _assessment.StartDateNotificationId = _assessment.CourseId * 20000 + _assessment.Id * 20 + 1;

        await AssessmentService.UpdateAssessment(_assessment);

        if (isNotificationEnabled)
        {
            var notification = new NotificationRequest()
            {
                NotificationId = _assessment.StartDateNotificationId,
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
            LocalNotificationCenter.Current.Clear(_assessment.StartDateNotificationId);
            _assessment.StartDateNotificationId = 0;
            await DisplayAlert("Notification Off", $"Start date notification for this assessment turned off. You will no longer be notified.", "OK");
        }

    }

    private async void OnDueNotificationToggled(object sender, ToggledEventArgs e)
    {
        if (!_isPageLoaded) return;

        var isNotificationEnabled = e.Value;

        _assessment.DueDateNotificationId = _assessment.CourseId * 20000 + _assessment.Id * 20 + 2;

        await AssessmentService.UpdateAssessment(_assessment);

        if (isNotificationEnabled)
        {
            var notification = new NotificationRequest()
            {
                NotificationId = _assessment.DueDateNotificationId,
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
            LocalNotificationCenter.Current.Clear(_assessment.DueDateNotificationId);
            _assessment.DueDateNotificationId = 0;
            await DisplayAlert("Notification Off", $"Due date notification for this assessment turned off. You will no longer be notified.", "OK");
        }
    }
}

