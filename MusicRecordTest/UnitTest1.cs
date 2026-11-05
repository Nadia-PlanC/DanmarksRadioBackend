using Danmarks_Radio.Models;
using Danmarks_Radio.Repos;

namespace MusicRecordTest
{
    public class UnitTest1
    {
        private readonly MusicListRepo _repo;

        public UnitTest1()
        {
            _repo = new MusicListRepo(includeData: false);
        }

        [Fact]
        public void GetAllTest()
        {
            MusicListRepo repo = new MusicListRepo(includeData: true);

            IEnumerable<MusicRecord> actual = repo.GetAll();

            Assert.Equal(4, actual.Count());
        }

        [Fact]
        public void GetAllTitleContainsTest()
        {
            MusicListRepo repo = new MusicListRepo(includeData: true);

            IEnumerable<MusicRecord> actual = repo.GetAll(titleContains: "Billie");

            Assert.Equal("Billie Jean", actual.First().Title);
        }

        [Fact]
        public void GetAllSortByYearTest()
        {
            MusicListRepo repo = new MusicListRepo(includeData: true);

            IEnumerable<MusicRecord> actual = repo.GetAll(sortBy: "year");

            Assert.Equal(1975, actual.First().PublicationYear);
        }

        [Fact]
        public void AddRecord_AssignsIdAndStores()
        {
            MusicRecord newRecord = new MusicRecord { Title = "TestRecord", Artist = "TestArtist", Duration = 335, PublicationYear = 2023 };

            MusicRecord added = _repo.AddRecord(newRecord); 

            int firstId = added.Id;
            Assert.Equal(1, firstId);
            Assert.Equal("TestRecord", added.Title);
            Assert.Equal("TestArtist", added.Artist);
            Assert.Equal(335, added.Duration);
            Assert.Equal(2023, added.PublicationYear);

            IEnumerable<MusicRecord> all = _repo.GetAllRecords();
            int countAfterAdd = all.Count();
            Assert.Equal(1, countAfterAdd);

            // Add second record and verify id increments
            MusicRecord secondRecord = new MusicRecord { Title = "Second", Artist = "SecondArtist", Duration = 360, PublicationYear = 2024 };
            MusicRecord addedSecond = _repo.AddRecord(secondRecord);
            Assert.Equal(2, addedSecond.Id);

            // Confirm original instance had its Id mutated by repository implementation
            Assert.Equal(1, newRecord.Id);
            Assert.Equal(2, secondRecord.Id);
        }

        [Fact]
        public void AddRecord_Null_ThrowsArgumentNullException()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => _repo.AddRecord(null!));
            Assert.Equal("record", ex.ParamName);
        }

        [Fact]
        public void RemoveRecord_RemovesAndReturns()
        {
            MusicRecord record = new MusicRecord { Title = "ToRemove", Artist = "ToRemove", Duration = 335, PublicationYear = 2023 };
            MusicRecord added = _repo.AddRecord(record);

            MusicRecord? removed = _repo.RemoveRecord(added.Id);
            Assert.NotNull(removed);
            Assert.Equal(added.Id, removed.Id);

            MusicRecord? postLookup = _repo.GetRecordById(added.Id);
            Assert.Null(postLookup);

            int remaining = _repo.GetAll().Count();
            Assert.Equal(0, remaining);
        }

        [Fact]
        public void RemoveRecord_NonExistent_ReturnsNull()
        {
            MusicRecord? removed = _repo.RemoveRecord(12345);
            Assert.Null(removed);
        }

        [Fact]
        public void UpdateRecord_UpdatesExisting_ReturnsUpdated()
        {
            //MusicListRepo repo = new MusicListRepo(includeData: false);
            MusicRecord original = new MusicRecord { Title = "Old", Artist = "OldArtist", Duration = 335, PublicationYear = 1985 };
            MusicRecord added = _repo.AddRecord(original);

            MusicRecord updatedInfo = new MusicRecord { Title = "NewTitle", Artist = "NewArtist", Duration = 360, PublicationYear = 1985 };
            MusicRecord? updated = _repo.UpdateRecord(added.Id, updatedInfo);

            Assert.NotNull(updated);
            Assert.Equal(added.Id, updated!.Id);
            Assert.Equal("NewTitle", updated.Title);
            Assert.Equal("NewArtist", updated.Artist);
            Assert.Equal(360, updated.Duration);
            Assert.Equal(1985, updated.PublicationYear);

            // Confirm repository stored the updated values
            MusicRecord? stored = _repo.GetRecordById(added.Id);
            Assert.NotNull(stored);
            Assert.Equal("NewTitle", stored!.Title);
            Assert.Equal("NewArtist", stored!.Artist);
            Assert.Equal(360, stored!.Duration);
            Assert.Equal(1985, stored!.PublicationYear);
        }

        [Fact]
        public void UpdateActress_NonExistent_ReturnsNull()
        {
            //MusicListRepo repo = new MusicListRepo(includeData: false);
            MusicRecord updatedInfo = new MusicRecord { Title = "Whatever", Artist = "Whatever", Duration = 360, PublicationYear = 1900 };

            MusicRecord? updated = _repo.UpdateRecord(7, updatedInfo);
            Assert.Null(updated);
        }
    }
}
