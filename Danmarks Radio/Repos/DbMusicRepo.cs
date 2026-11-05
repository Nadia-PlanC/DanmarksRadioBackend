using Danmarks_Radio.EFDB;
using Danmarks_Radio.Interfaces;
using Danmarks_Radio.Models;

namespace Danmarks_Radio.Repos
{
    public class DbMusicRepo : IMusicRepo
    {
        private readonly EFDBContext _context;

        public DbMusicRepo(EFDBContext context)
        {
            _context = context;
        }

        public IEnumerable<MusicRecord> GetAllRecords()
        {
            return _context.MusicRecords.ToList();
        }

        public IEnumerable<MusicRecord> GetAll(string? titleContains = null, string? artist = null, string? sortBy = null)
        {
            IEnumerable<MusicRecord> result = _context.MusicRecords.ToList();

            if (titleContains != null)
            {
                result = result.Where(s => s.Title != null && s.Title.Contains(titleContains, StringComparison.OrdinalIgnoreCase));
            }

            if (artist != null)
            {
                result = result.Where(s => s.Artist != null && s.Artist.Equals(artist, StringComparison.OrdinalIgnoreCase));
            }

            switch (sortBy)
            {
                case null:
                    return result;
                case "title":
                    return result.OrderBy(s => s.Title);
                case "titleDesc":
                    return result.OrderByDescending(s => s.Title);
                case "year":
                    return result.OrderBy(s => s.PublicationYear);
                case "yearDesc":
                    return result.OrderByDescending(s => s.PublicationYear);
                default:
                    throw new ArgumentException("Illegal sorting: " + sortBy);
            }
        }

        public MusicRecord? GetRecordById(int id)
        {
            MusicRecord? record = _context.MusicRecords.FirstOrDefault(r => r.Id == id);

            return record;
        }

        public MusicRecord AddRecord(MusicRecord record)
        {
            record.Id = 0; // databasen laver Id'et
            _context.MusicRecords.Add(record);
            _context.SaveChanges();
            return record;
        }

        public MusicRecord? UpdateRecord(int id, MusicRecord updatedRecord)
        {
            MusicRecord? existing = GetRecordById(id);
            if (existing == null)
            {
                return null;
            }
            existing.Title = updatedRecord.Title;
            existing.Artist = updatedRecord.Artist;
            existing.Duration = updatedRecord.Duration;
            existing.PublicationYear = updatedRecord.PublicationYear;
            _context.SaveChanges();
            return existing;
        }

        public MusicRecord? RemoveRecord(int id)
        {
            MusicRecord? record = GetRecordById(id);
            if (record == null)
            {
                return null;
            }
            _context.MusicRecords.Remove(record);
            _context.SaveChanges();
            return record;
        }
    }
}