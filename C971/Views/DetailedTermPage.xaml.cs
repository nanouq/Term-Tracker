using C971.Model;
using C971.Services;

namespace C971.Views;

public partial class DetailedTermPage : ContentPage
{
	private Term _term;
	public DetailedTermPage(Term term)
	{
		InitializeComponent();
		_term = term;
		LoadTermDetails();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await LoadTermDetails();
		await LoadCourses();	
	}

	private async Task LoadTermDetails()
	{
		var updatedTerm = await TermService.GetById(_term.Id);

		if (updatedTerm != null)
		{
			_term = updatedTerm;
			BindingContext = _term;
		}
	}

	private async Task LoadCourses()
	{
		var courses = await CourseService.GetByTermId(_term.Id);
		CoursesCollectionView.ItemsSource = courses;
		if(courses.Any())
		{
			NoCoursesAdded.IsVisible = false;
		}
		else
		{
			NoCoursesAdded.IsVisible= true;
		}
	}

	private async void OnEditClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new EditTermPage(_term));
	}

	private async void OnDeleteClicked(object sender, EventArgs e)
	{
		bool confirmDelete = await DisplayAlert(
			"Confirm Deletion",
			"Are you sure you want to delete this term and all associated courses?",
			"Yes",
			"No");

		if (confirmDelete)
		{
			await TermService.RemoveTerm(_term.Id);

			await DisplayAlert("Success", "The term has been deleted", "OK");

			await Navigation.PopAsync();
		}
	}

    private async void OnAddCourseClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddCoursePage(_term));
    }

	private async void OnCourseSelected(object sender, SelectionChangedEventArgs e)
	{
		if(e.CurrentSelection.FirstOrDefault() is Course selectedCourse)
		{
			await Navigation.PushAsync(new DetailedCoursePage(selectedCourse));
		}

		((CollectionView)sender).SelectedItem = null;
	}
}