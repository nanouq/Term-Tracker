using C971.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971.Services
{
    public static class TermService
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

            await db.CreateTableAsync<Term>();
        }

        public static async Task AddTerm(string title, DateTime start, DateTime end)
        {
            await Init();
            var term = new Term()
            {
                Title = title,
                StartDate = start,
                EndDate = end
            };

            var id = await db.InsertAsync(term);
        }

        public static async Task RemoveTerm(int id)
        {
            await Init();

            await CourseService.RemoveByTermId(id);
            await db.DeleteAsync<Term>(id);

        }

        public static async Task UpdateTerm(int id, string title, DateTime start, DateTime end)
        {
            await Init();

            var termQuery = await db.Table<Term>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();

            if (termQuery != null)
            {
                termQuery.Title = title;
                termQuery.StartDate = start;
                termQuery.EndDate = end;

                await db.UpdateAsync(termQuery);               
            }
        }

        public static async Task<IEnumerable<Term>> GetTerm()
        {
            await Init();

            var term = await db.Table<Term>().ToListAsync();
            return term;
        }

        public static async Task<Term> GetById(int id)
        {
            return await db.Table<Term>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
