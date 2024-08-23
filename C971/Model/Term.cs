using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace C971.Model
{
    public class Term
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("start")]
        public DateTime StartDate { get; set; }

        [Column("end")]
        public DateTime EndDate { get; set; }

        
        public string DateRange => $"{StartDate:MMMM dd, yyyy} - {EndDate:MMMM dd, yyyy}";

        [Ignore]
        public ObservableCollection<Course> Courses { get; set; } = new ObservableCollection<Course>();
    }
}
