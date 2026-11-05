using Danmarks_Radio.Interfaces;
using Danmarks_Radio.Models;

namespace Danmarks_Radio.Repos
{
    public class UserListRepo : IUserRepo
    {
        private readonly List<User> _users = new();

        public UserListRepo(bool includeData = false)
        {
            if (includeData)
            {
                _users.Add(new User { Id = 1, Username = "admin", Password = "1234", Role = "Admin" });
                _users.Add(new User { Id = 2, Username = "henrik", Password = "secret", Role = "User" });
            }
        }

        public User? GetByUsername(string username)
        {
            User? user = _users.AsReadOnly().FirstOrDefault(u => u.Username == username);

            return user;
        }
    }
}