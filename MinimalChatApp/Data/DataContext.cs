using MinimalChatApp.Models;

namespace MinimalChatApp.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
