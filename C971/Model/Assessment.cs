using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971.Model
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }

        public AssessmentType Type { get; set; }
        public int StartDateNotificationId { get; set; }
        public int DueDateNotificationId { get; set; }

        public int CourseId { get; set; }

        public string AssessmentDateRange => $"{StartDate:MMMM dd, yyyy} - {DueDate:MMMM dd, yyyy}";
    }

    public enum AssessmentType
    {
        Performance,
        Objective
    }
}
