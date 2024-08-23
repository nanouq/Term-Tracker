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

		await TermService.UpdateTerm(_term.Id, _term.Title, _term.StartDate, _term.EndDate);

		await Navigation.PopAsync();
	}
}