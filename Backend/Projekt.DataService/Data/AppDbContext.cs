﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projekt.Entities.Models;

namespace Projekt.DataService.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Category> ForumCategories { get; set; }
        public DbSet<Subcategory> ForumSubcategories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> ForumComments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<BookImage> BookImages { get; set; }
        public DbSet<GalleryImage> GalleryImages { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<GalleryDisplaySequence> GalleryDisplaySequence { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<BookInstance> BookInstances { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehaviour", true);

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Subcategory>()
                .HasOne(c => c.Category)
                .WithMany(s => s.Subcategories)
                .HasForeignKey(c => c.CategoryId)
                .IsRequired();
            builder.Entity<Post>()
                .HasOne(s => s.Subcategory)
                .WithMany(p => p.Posts )
                .HasForeignKey(c => c.SubCategoryId);
            builder.Entity<Post>()
                .HasOne(u => u.AppUser)
                .WithMany(p => p.Posts)
                .HasForeignKey(k => k.UserId);
            builder.Entity<Comment>()
                .HasOne(p => p.Post)
                .WithMany(c => c.Comments )
                .HasForeignKey(k => k.PostId);
            builder.Entity<Comment>()
                .HasOne(u => u.AppUser)
                .WithMany(c => c.Comments)
                .HasForeignKey(k => k.UserId);
            builder.Entity<Image>()
                .HasDiscriminator<string>("ImageType")
                .HasValue<BookImage>("BookImage")
                .HasValue<GalleryImage>("GalleryImage");
            builder.Entity<BookImage>()
                .HasOne(b => b.Book)
                .WithMany(i => i.Images)
                .HasForeignKey(b => b.BookId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Review>()
                .HasOne(u => u.AppUser)
                .WithMany(c => c.Reviews)
                .HasForeignKey(k => k.UserId);
        }
    }
}
