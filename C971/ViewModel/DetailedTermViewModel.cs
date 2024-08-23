using C971.Model;
using C971.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace C971.ViewModel;

public class DetailedTermViewModel : ObservableObject
{
	/*private Term _term;*/
	public Term SelectedTerm { get; set; }
	
	public DetailedTermViewModel(Term term)
	{
		SelectedTerm = term;
		LoadCourses(term.Id);
	}

	private async void LoadCourses(int id)
	{
		var courses = await CourseService.GetByTermId(id);
		SelectedTerm.Courses = new ObservableCollection<Course>(courses);
		OnPropertyChanged(nameof(SelectedTerm));
	}
}