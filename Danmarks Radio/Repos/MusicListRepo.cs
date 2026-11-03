using Danmarks_Radio.Interfaces;
using Danmarks_Radio.Models;

namespace Danmarks_Radio.Repos
{
    public class MusicListRepo : IMusicListRepo
    {
        private readonly List<MusicRecord> _records = new();
        private int _nextId = 1;

        public MusicListRepo(bool includeData = false)
        {
            if (includeData)
            {
                _records.Add(new MusicRecord { Title = "Bohemian Rhapsody", Artist = "Queen", Duration = 5.55f, Year = 1975 });
                _records.Add(new MusicRecord { Title = "Billie Jean", Artist = "Michael Jackson", Duration = 4.54f, Year = 1982 });
                _records.Add(new MusicRecord { Title = "Smells Like Teen Spirit", Artist = "Nirvana", Duration = 5.01f, Year = 1991 });
                _records.Add(new MusicRecord { Title = "Blinding Lights", Artist = "The Weeknd", Duration = 3.20f, Year = 2019 });
            }
        }

        public IEnumerable<MusicRecord> GetAllRecords()
        {
            return new List<MusicRecord>(_records);
        }

        public IEnumerable<MusicRecord> GetAll(string? titleContains = null, string? artist = null, string? sortBy = null)
        {
            IEnumerable<MusicRecord> result = new List<MusicRecord>(_records);

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
                    return result.OrderBy(s => s.Year);
                case "yearDesc":
                    return result.OrderByDescending(s => s.Year);
                default:
                    throw new ArgumentException("Illegal sorting: " + sortBy);
            }
        }

        public MusicRecord? GetRecordById(int id)
        {
            return _records.FirstOrDefault(r => r.Id == id);
        }
        public MusicRecord AddRecord(MusicRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            record.Id = _nextId++;
            _records.Add(record);
            return record;
        }

        public MusicRecord? RemoveRecord(int id)
        {
            var record = GetRecordById(id);
            if (record != null)
            {
                _records.Remove(record);
                return record;
            }
            return null;
        }

        public MusicRecord? UpdateRecord(int id, MusicRecord updatedRecord)
        {
            var existingRecord = GetRecordById(id);
            if (existingRecord != null)
            {
                existingRecord.Title = updatedRecord.Title;
                existingRecord.Artist = updatedRecord.Artist;
                existingRecord.Duration = updatedRecord.Duration;
                existingRecord.Year = updatedRecord.Year;
                return existingRecord;
            }
            return null;
        }

       

    }
}




