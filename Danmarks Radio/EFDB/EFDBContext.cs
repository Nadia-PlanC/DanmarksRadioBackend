using Danmarks_Radio.Models;
using Microsoft.EntityFrameworkCore;

namespace Danmarks_Radio.EFDB
{
    public class EFDBContext : DbContext
    {
        public EFDBContext(DbContextOptions<EFDBContext> options) : base(options) { }

        public DbSet<MusicRecord> MusicRecords { get; set; }
        public DbSet<User> Users { get; set; }
    }
}