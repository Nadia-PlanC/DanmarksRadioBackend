using Danmarks_Radio.Models;
using Microsoft.EntityFrameworkCore;

namespace Danmarks_Radio.DrDbContext
{
    public class MusicRecordDbContext : DbContext
    {
        public MusicRecordDbContext(DbContextOptions<MusicRecordDbContext> options) : base(options)
        {
        }
        public DbSet<MusicRecord> MusicRecords { get; set; } 
    }
}
