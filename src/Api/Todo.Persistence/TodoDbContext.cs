using Microsoft.EntityFrameworkCore;
using Todo.Domain;

namespace Todo.Persistence
{
    public class TodoDbContext : DbContext
    {
        public DbSet<TodoRecord> TodoRecords { get; set; } = null!;

        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<TodoRecord>();
            entity.HasKey(tr => tr.Id);
            entity.Property(tr => tr.Id).ValueGeneratedNever();
        }
    }
}
