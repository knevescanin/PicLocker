using BlazorApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Areas.Identity.Data;

public class BlazorAppIdentityDbContext : IdentityDbContext<ApplicationUser>
{

    public BlazorAppIdentityDbContext(DbContextOptions<BlazorAppIdentityDbContext> options)
        : base(options)
    {
    }

    public DbSet<ImageFile> ImageFiles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var admin = new IdentityRole("admin");
        admin.NormalizedName = "admin";

        var user = new IdentityRole("user");
        user.NormalizedName = "user";

        builder.Entity<IdentityRole>()
        .HasData(admin, user);

        builder.Entity<ApplicationUser>()
            .Property(u => u.FirstName)
            .HasMaxLength(50);

        builder.Entity<ApplicationUser>()
            .Property(u => u.LastName)
            .HasMaxLength(50);

            builder.Entity<ApplicationUser>()
            .Property(u => u.UserName)
            .HasMaxLength(50);

        builder.Entity<ApplicationUser>()
        .HasMany(u => u.ImageFiles)
        .WithOne(i => i.User)
        .HasForeignKey(i => i.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ImageFile>().ToTable("ImageFile");

    }
}
