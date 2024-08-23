using C971.Model;
using C971.Services;
using C971.ViewModel;
using System.Collections.ObjectModel;
using C971.Views;

namespace C971
{
    public partial class MainPage : ContentPage
    {

        private MainViewModel _viewModel;
        public MainPage()
        {
            InitializeComponent();

            _viewModel = new MainViewModel();
            BindingContext = _viewModel;
        }

        private async void OnTermSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Term selectedTerm)
            {
                await Navigation.PushAsync(new DetailedTermPage(selectedTerm));

                ((CollectionView)sender).SelectedItem = null;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _viewModel.LoadTerms();
        }

        private async void OnAddTermClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTermPage());
        }


    }
}
