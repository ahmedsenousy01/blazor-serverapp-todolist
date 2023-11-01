using Microsoft.EntityFrameworkCore;
using ServerApp.Data.Models;
using ServerApp.Data.Services.Security;

namespace ServerApp.Data.Services
{
    public class UserService
    {
        private readonly AppDbContext db;

        public UserService(AppDbContext context)
        {
            db = context;
        }

        public async Task<Role?> GetRoleByValue(string value)
        {
            return await db.Roles.FirstOrDefaultAsync(r => r.Value == value);
        }

        public async Task<Role?> GetRoleById(int id)
        {
            return await db.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await db.Users.Include(u => u.TodoList).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await db.Users.Include(u => u.TodoList).Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await db.Users.Include(u => u.TodoList).Include(u => u.Role).FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task CreateAsync(User user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            user.Role = await GetRoleById(user.RoleId);

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, User reqBody)
        {
            if (reqBody == null) throw new ArgumentNullException(nameof(reqBody));

            var existingUser = await GetByIdAsync(id);
            if (existingUser is not null)
            {
                existingUser.UserName = reqBody.UserName ?? existingUser.UserName;
                existingUser.PasswordHash = HashingService.HashPassword(reqBody.PasswordHash) ?? existingUser.PasswordHash;

                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string id)
        {
            var user = await GetByIdAsync(id);
            if (user is not null)
            {
                db.Users.Remove(user);
                await db.SaveChangesAsync();
            }
        }
    }
}
