using C971.Views;

namespace C971
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromArgb("#003366"),
                BarTextColor = Colors.White,
            };
        }
    }
}
