using Danmarks_Radio.Interfaces;
using Danmarks_Radio.Models;
using Danmarks_Radio.Repos;

namespace MusicRecordXUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void GetAll()
        {
            MusicListRepo repo = new MusicListRepo(includeData: true);

            IEnumerable<MusicRecord> actual = repo.GetAll();

            Assert.Equal(4, actual.Count());
        }

        [Fact]
        public void GetAllName()
        {
            MusicListRepo repo = new MusicListRepo(includeData: true);

            IEnumerable<MusicRecord> actual = repo.GetAll(titleContains: "Bohemian Rhapsody");

            Assert.Equal("Bohemian Rhapsody", actual.First().Title);
        }

        [Fact]
        public void GetAllYear()
        {
            MusicListRepo repo = new MusicListRepo(includeData: true);

            IEnumerable<MusicRecord> actual = repo.GetAll(sortBy: "year");

            Assert.Equal(1975, actual.First().Year);
        }
    }
}
