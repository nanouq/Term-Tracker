using C971.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971.Services
{
    public static class DatabaseSeeder
    {
        public static async Task SeedData()
        {
            Debug.WriteLine("SeedData was successfully called");
            var existingTerm = await TermService.GetTermByName("Fall 2024");
            if (existingTerm != null)
            {
                return;
            }

            //Create Term For Evaluation Purposes
            string termName = "Fall 2024";
            DateTime termStart = new DateTime(2024, 8, 26);
            DateTime termEnd = new DateTime(2024, 12, 9);

            await TermService.AddTerm(termName, termStart, termEnd);

            var newTerm = await TermService.GetTermByName(termName);

            //Create Course For Evaluation Purposes
            var course = new Course
            {
                Name = "C971",
                StartDate = new DateTime(2024, 9, 1),
                EndDate = new DateTime(2024, 10, 1),
                Status = "In Progress",
                InstructorName = "Anika Patel",
                InstructorPhone = "555-123-4567",
                InstructorEmail = "anika.patel@strimeuniversity.edu",
                TermId = newTerm.Id,
                Notes = "This is a sample note",
                PerformanceAssessment = true,
                ObjectiveAssessment = true
            };

            await CourseService.AddCourse(course);
            var newCourse = await CourseService.GetCourseByName(course.Name);

            //Create Performance Assessment For Evaluation Purposes
            var pAssessment = new Assessment
            {
                Name = "Sample Performance Assessment",
                StartDate = new DateTime(2024, 9, 1),
                DueDate = new DateTime(2024, 9, 15),
                Type = AssessmentType.Performance,
                CourseId = newCourse.Id
            };

            await AssessmentService.AddAssessmentWithoutCourse(pAssessment);

            var oAssessment = new Assessment
            {
                Name = "Sample Objective Assessment",
                StartDate = new DateTime(2024, 9, 16),
                DueDate = new DateTime(2024, 10, 1),
                Type = AssessmentType.Objective,
                CourseId = newCourse.Id
            };

            await AssessmentService.AddAssessmentWithoutCourse(oAssessment);
        }
    }
}
