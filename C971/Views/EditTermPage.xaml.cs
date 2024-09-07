using C971.Model;
using C971.Services;

namespace C971.Views;

public partial class EditTermPage : ContentPage
{
	private Term _term;
	public EditTermPage(Term term)
	{
		InitializeComponent();
		_term = term;

		TermTitleEntry.Text = _term.Title;
		StartDatePicker.Date = _term.StartDate;
		EndDatePicker.Date = _term.EndDate;
	}

	private async void OnSaveTermClicked(object sender, EventArgs e)
	{
		_term.Title = TermTitleEntry.Text;
		_term.StartDate = StartDatePicker.Date;
		_term.EndDate = EndDatePicker.Date;

		if(string.IsNullOrWhiteSpace(_term.Title) )
		{
            await DisplayAlert("Error", "Please enter a term title.", "OK");
            return;
        }

		if(_term.EndDate <= _term.StartDate)
		{
            await DisplayAlert("Error", "End date must be after the start date.", "OK");
            return;
        }

		await TermService.UpdateTerm(_term.Id, _term.Title, _term.StartDate, _term.EndDate);
        await DisplayAlert("Success", "Successfully updated term", "OK");
        await Navigation.PopAsync();
	}
}