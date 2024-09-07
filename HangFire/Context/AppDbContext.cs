using Microsoft.EntityFrameworkCore;

namespace HangFireApp.Context
{
    public sealed class AppDbContext : DbContext //sealed bir sınıftan miras alınamaz
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
