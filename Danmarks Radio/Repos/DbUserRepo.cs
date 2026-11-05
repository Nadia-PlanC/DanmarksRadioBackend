using Danmarks_Radio.EFDB;
using Danmarks_Radio.Interfaces;
using Danmarks_Radio.Models;

namespace Danmarks_Radio.Repos
{
    public class DbUserRepo : IUserRepo
    {
        private readonly EFDBContext _context;

        public DbUserRepo(EFDBContext context)
        {
            _context = context;
        }

        public User? GetByUsername(string username)
        {
            User? user = _context.Users.FirstOrDefault(u => u.Username == username);

            return user;
        }
    }
}