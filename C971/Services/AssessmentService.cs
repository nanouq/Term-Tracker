
using C971.Model;
using Plugin.LocalNotification;
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

        public static async Task AddAssessmentWithoutCourse(Assessment assessment)
        {
            await Init();
            await db.InsertAsync(assessment);
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

        public static async Task UpdateAssessment(Assessment assessment)
        {
            await Init();
            await db.UpdateAsync(assessment);
        }

        public static async Task RemoveAssessment(Assessment assessment)
        {
            await Init();
            await db.DeleteAsync<Assessment>(assessment.Id);

            Course course = await CourseService.GetById(assessment.CourseId);

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

        public static async Task DeleteAllCourseAssessments(Course course)
        {
            await Init();

            if (course.PerformanceAssessment)
            {
                Assessment pAssessment = await GetAssessmentByType(course.Id, AssessmentType.Performance);

                if (pAssessment.StartDateNotificationId != 0)
                {
                    LocalNotificationCenter.Current.Clear(pAssessment.StartDateNotificationId);
                }
                if (pAssessment.DueDateNotificationId != 0)
                {
                    LocalNotificationCenter.Current.Clear(pAssessment.DueDateNotificationId);
                }

                await db.DeleteAsync<Assessment>(pAssessment.Id);
            }

            if (course.ObjectiveAssessment)
            {
                Assessment oAssessment = await GetAssessmentByType(course.Id, AssessmentType.Objective);

                if (oAssessment.StartDateNotificationId != 0)
                {
                    LocalNotificationCenter.Current.Clear(oAssessment.StartDateNotificationId);
                }
                if (oAssessment.DueDateNotificationId != 0)
                {
                    LocalNotificationCenter.Current.Clear(oAssessment.DueDateNotificationId);
                }
                await db.DeleteAsync<Assessment>(oAssessment.Id);
            }

        }

        public static async Task<Assessment> GetAssessmentByType(int courseId, AssessmentType type)
        {
            await Init();
            return await db.Table<Assessment>().Where(a => a.CourseId == courseId && a.Type == type).FirstOrDefaultAsync();
        }
    }

}
