using C971.Model;
using C971.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971.ViewModel
{
    
    public class MainViewModel : ObservableObject
    {
        public ObservableCollection<Term> Terms { get; set; }

       
        public MainViewModel()
        {
            Terms = new ObservableCollection<Term>();
        }


        public async Task LoadTerms()
        {
            var terms = await TermService.GetTerm();
            Terms.Clear();
            foreach(var term in terms)
            {
                Terms.Add(term);
            }

        }

        /*public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }*/
    }
}
