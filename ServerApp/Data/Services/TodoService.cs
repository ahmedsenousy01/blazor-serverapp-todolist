using Microsoft.EntityFrameworkCore;
using ServerApp.Data.Models;

namespace ServerApp.Data.Services
{
    public class TodoService
    {
        private readonly AppDbContext db;
        public TodoService(AppDbContext db)
        {
            this.db = db;
        }

        public async Task<List<Todo>?> GetTodosAsync()
        {
            return await db.Todos.ToListAsync();
        }

        public async Task<Todo?> GetByIdAsync(string id)
        {
            return await db.Todos.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Todo>?> GetTodosByUserIdAsync(string id)
        {
            return await db.Todos.Where(t => t.UserId == id).ToListAsync();
        }

        public async Task CreateAsync(Todo todo)
        {
            if (todo is null) throw new ArgumentNullException(nameof(todo));

            await db.Todos.AddAsync(todo);
            await db.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, Todo newTodo)
        {
            if (newTodo == null) throw new ArgumentNullException(nameof(newTodo));

            var existingTodo = await GetByIdAsync(id);
            if (existingTodo is not null)
            {
                if (newTodo.Title is not null)
                {
                    existingTodo.Title = newTodo.Title;
                }

                if (newTodo.Description is not null)
                {
                    existingTodo.Description = newTodo.Description;
                }


                existingTodo.LastUpdatedAt = DateTime.UtcNow;
                await db.SaveChangesAsync();
            }
        }

        public async Task MarkTodoAsCompleted(string id)
        {
            var todo = await GetByIdAsync(id);
            if (todo is null) throw new ArgumentNullException(id);

            todo.IsCompleted = !todo.IsCompleted;
            todo.LastUpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var todo = await GetByIdAsync(id);
            if (todo is not null)
            {
                db.Todos.Remove(todo);
                await db.SaveChangesAsync();
            }
        }
    }
}
