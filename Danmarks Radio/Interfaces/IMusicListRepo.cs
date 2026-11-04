using Danmarks_Radio.Models;
using Microsoft.AspNetCore.Mvc;

namespace Danmarks_Radio.Interfaces
{
    public interface IMusicListRepo
    {
        public IEnumerable<MusicRecord> GetAllRecords();
        public IEnumerable<MusicRecord> GetAll(string? titleContains = null, string? artist = null, string? sortBy = null);
        public MusicRecord? GetRecordById(int id);
        public MusicRecord AddRecord(MusicRecord record);
        public MusicRecord? RemoveRecord(int id);
        public MusicRecord? UpdateRecord(int id, MusicRecord updatedRecord);
    }
}