using Microsoft.EntityFrameworkCore;
using TutorialProjectAPI.Models;

namespace TutorialProjectAPI.Contexts
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
            // todo
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PostDB>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReplyDB>()
                .HasOne(r => r.Post)
                .WithMany(p => p.Replies)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReplyDB>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PostDB>()
                .HasOne(p => p.Image)
                .WithMany()
                .HasForeignKey(p => p.ImageId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<UserDB>()
                .HasOne(u => u.ProfileImage)
                .WithMany()
                .HasForeignKey(u => u.ProfileImageId)
                .OnDelete(DeleteBehavior.SetNull);
        }
        public DbSet<UserDB> Users { get; set; }
        public DbSet<PostDB> Posts { get; set; }
        public DbSet<ReplyDB> Replies { get; set; }
        public DbSet<ImageDB> Images { get; set; }
    }

    public class CreatePostDto
    {
        public string Body { get; set; }
        public Guid UserId { get; set; }
        public string? ImageBase64 { get; set; } // Optional: for uploading a new image
    }
}
