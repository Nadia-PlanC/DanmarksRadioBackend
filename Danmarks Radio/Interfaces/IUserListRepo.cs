using Danmarks_Radio.Models;

namespace Danmarks_Radio.Interfaces
{
    public interface IUserListRepo
    {
        User? GetByUsername(string username);
    }
}