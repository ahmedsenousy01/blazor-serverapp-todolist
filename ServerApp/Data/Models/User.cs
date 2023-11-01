using ServerApp.Data.Services;

namespace ServerApp.Data.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int RoleId { get; set; } = 2;
        public Role Role { get; set; } = null!;
        public List<Todo> TodoList { get; set; } = new List<Todo>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
