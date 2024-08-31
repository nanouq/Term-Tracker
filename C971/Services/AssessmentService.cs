using C971.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971.Services
{
    public static class AssessmentService
    {
        static SQLiteAsyncConnection db;

        static async Task Init()
        {
            if (db != null)
            {
                return;
            }
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<Assessment>();
        }

        public static async Task AddAssessment(Assessment assessment, Course course)
        {
            await Init();
            await db.InsertAsync(assessment);

            if(assessment.Type == AssessmentType.Performance)
            {
                course.PerformanceAssessment = true;
                await CourseService.UpdateCourse(course);
            }

            if (assessment.Type == AssessmentType.Objective)
            {
                course.ObjectiveAssessment = true;
                await CourseService.UpdateCourse(course);
            }
        }

        public static async Task RemoveCourse(Assessment assessment, Course course)
        {
            await Init();
            await db.DeleteAsync<Assessment>(assessment.Id);

            if (assessment.Type == AssessmentType.Performance)
            {
                course.PerformanceAssessment = false;
                await CourseService.UpdateCourse(course);
            }

            if (assessment.Type == AssessmentType.Objective)
            {
                course.ObjectiveAssessment = false;
                await CourseService.UpdateCourse(course);
            }
        }

        public static async Task<Assessment> GetAssessmentByType(int courseId, AssessmentType type)
        {
            await Init();
            return await db.Table<Assessment>().Where(a => a.CourseId == courseId && a.Type == type).FirstOrDefaultAsync();
        }
    }

}
