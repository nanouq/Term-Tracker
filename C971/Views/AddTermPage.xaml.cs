using C971.Services;

namespace C971.Views;

public partial class AddTermPage : ContentPage
{
	public AddTermPage()
	{
		InitializeComponent();
	}

	private async void OnSaveTermClicked(object sender, EventArgs e)
	{
		string title = TermTitleEntry.Text;
		DateTime startDate = StartDatePicker.Date;
        DateTime endDate = EndDatePicker.Date;

		if(string.IsNullOrWhiteSpace(title))
		{
			await DisplayAlert("Error", "Please enter a term title.", "OK");
			return;
		}

		if(endDate <= startDate)
		{
			await DisplayAlert("Error", "End date must be after the start date.", "OK");
			return;
		}

		await TermService.AddTerm(title, startDate, endDate);
        await DisplayAlert("Success", "Successfully added term", "OK");
        await Navigation.PopAsync();
    }
}