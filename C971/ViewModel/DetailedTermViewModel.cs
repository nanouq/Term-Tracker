using C971.Model;
using CommunityToolkit.Mvvm.ComponentModel;

namespace C971.ViewModel;

public class DetailedTermViewModel : ObservableObject
{
	private Term _term;
	public Term Term
	{
		get => _term;
		set
		{
			_term = value;
			OnPropertyChanged();
		}
	}
	public DetailedTermViewModel(Term term)
	{
		Term = term;
	}
}