using Danmarks_Radio.Models;

namespace Danmarks_Radio.Interfaces
{
    public interface IUserRepo
    {
        User? GetByUsername(string username);
    }
}