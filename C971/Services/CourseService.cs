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
    public static class CourseService
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

            await db.CreateTableAsync<Course>();
        }

        public static async Task AddCourse(Course course)
        {
            await Init();
            var id = await db.InsertAsync(course);

            if (id > 0)
            {
                Debug.WriteLine($"Course added successfully with id of {id}");
            }
            else
            {
                Debug.WriteLine("Faled to add course.");
            }
        }

        public static async Task RemoveCourse(int id)
        {
            await Init();

            Course course = await GetById(id);

            if (course.StartDateNotificationId != 0)
            {
                LocalNotificationCenter.Current.Clear(course.StartDateNotificationId);
            }
            if(course.EndDateNotificationId != 0)
            {
                LocalNotificationCenter.Current.Clear(course.EndDateNotificationId);
            }

            await db.DeleteAsync<Course>(id);
        }

        public static async Task RemoveByTermId(int id)
        {
            await Init();

            await db.Table<Course>()
                .Where(c => c.TermId == id)
                .DeleteAsync();
        }

        public static async Task UpdateCourse(Course course)
        {
            await Init();
            await db.UpdateAsync(course);
        }

        public static async Task<Course> GetById(int id)
        {
            await Init();
            return await db.Table<Course>().Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public static async Task<IEnumerable<Course>> GetByTermId(int id)
        {
            await Init();

            return await db.Table<Course>().Where(c => c.TermId == id).ToListAsync();
        }

        public static async Task<IEnumerable<Course>> GetCourse()
        {
            await Init();

            var course = await db.Table<Course>().ToListAsync();
            return course;
        }

        public static async Task<Course> GetCourseByName(string name)
        {
            return await db.Table<Course>().FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
