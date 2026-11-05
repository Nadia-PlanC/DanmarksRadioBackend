using Danmarks_Radio.Interfaces;
using Danmarks_Radio.Models;

namespace Danmarks_Radio.DrDbContext
{
    public class MusicListRepoDb : IMusicListRepo
    {
        private readonly MusicRecordDbContext _context;

        public MusicListRepoDb(MusicRecordDbContext context)
        {
            _context = context;
        }

        public IEnumerable<MusicRecord> GetAllRecords()
        {
            return _context.MusicRecords.ToList().AsReadOnly();
        }

        public IEnumerable<MusicRecord> GetAll(string? titleContains = null, string? artist = null, string? sortBy = null)
        {
            IEnumerable<MusicRecord> result = _context.MusicRecords;

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
                    return result.ToList().AsReadOnly();
                case "title":
                    return result.OrderBy(s => s.Title).ToList().AsReadOnly();
                case "titleDesc":
                    return result.OrderByDescending(s => s.Title).ToList().AsReadOnly();
                case "year":
                    return result.OrderBy(s => s.Year).ToList().AsReadOnly();
                case "yearDesc":
                    return result.OrderByDescending(s => s.Year).ToList().AsReadOnly();
                default:
                    throw new ArgumentException("Illegal sorting: " + sortBy);
            }
        }

        public MusicRecord? GetRecordById(int id)
        {
            return _context.MusicRecords.FirstOrDefault(p => p.Id == id);
        }

        public MusicRecord AddRecord(MusicRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            record.Id = 0; // databasen tildeler id'et
            _context.MusicRecords.Add(record);
            _context.SaveChanges();
            return record;
        }

        public MusicRecord? RemoveRecord(int id)
        {
            MusicRecord? record = GetRecordById(id);
            if (record is null)
            {
                return null;
            }
            _context.MusicRecords.Remove(record);
            _context.SaveChanges();
            return record;
        }

        public MusicRecord? UpdateRecord(int id, MusicRecord updatedRecord)
        {
            MusicRecord? existingRecord = GetRecordById(id);
            if (existingRecord != null)
            {
                existingRecord.Title = updatedRecord.Title;
                existingRecord.Artist = updatedRecord.Artist;
                existingRecord.Duration = updatedRecord.Duration;
                existingRecord.Year = updatedRecord.Year;
                _context.SaveChanges();
                return existingRecord;
            }
            return null;
        }
    }
}
