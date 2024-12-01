using Microsoft.EntityFrameworkCore;
using PresenterCorner.Models;

namespace PresenterCorner.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Presentation> Presentations { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<SlideElement> SlideElements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Presentation)
                .WithMany(p => p.Users)
                .HasForeignKey(u => u.PresentationId)
                .IsRequired(false); // Makes the foreign key optional
        


        modelBuilder.Entity<User>()
                .HasIndex(u => u.Nickname)
                .IsUnique();
            // Relationships and constraints
            modelBuilder.Entity<Presentation>()
                .HasMany(p => p.Slides)
                .WithOne()
                .HasForeignKey(s => s.PresentationId);

            modelBuilder.Entity<Slide>()
                .HasMany(s => s.Elements)
                .WithOne()
                .HasForeignKey(e => e.SlideId);
        }
    }
}
