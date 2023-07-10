using Microsoft.EntityFrameworkCore;

using BookStore.Model;

namespace BookStore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)

        { 
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<Publisher> Publisher { get; set; }

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // one to many mapping as multiple books are there : for one author
            modelBuilder.Entity<Books>()
            .HasOne(_ => _.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(p => p.Id);

            // multiple books are there for one publisher
            modelBuilder.Entity<Books>()
            .HasOne(_ => _.Publisher)
            .WithMany(a => a.Books)
            .HasForeignKey(p => p.Id);

            modelBuilder.Entity<Books>()
           .HasOne(b => b.Author)
           .WithMany(a => a.Books)
           .HasForeignKey(b => b.AuthorId);


            modelBuilder.Entity<Books>()
           .HasOne(b => b.Publisher)
           .WithMany(p => p.Books)
           .HasForeignKey(b => b.PublisherId);
           







        }

    }
}
