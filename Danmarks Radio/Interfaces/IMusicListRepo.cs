using Danmarks_Radio.Models;

namespace Danmarks_Radio.Interfaces
{
    public interface IMusicListRepo
    {
        IEnumerable<MusicRecord> GetAll(string? titleContains = null, string? artist = null, string? sortBy = null);
    }
}