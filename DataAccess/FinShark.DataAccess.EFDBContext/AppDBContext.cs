using FinShark.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinShark.DataAccess.EFDBContext;

public class AppDBContext : IdentityDbContext<IdentityUser>
{
    #region constructor
    public AppDBContext(DbContextOptions options) : base(options) { }
    #endregion

    #region properties
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }
    #endregion

    #region methods
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Ignore<User>();

        builder.Entity<Portfolio>(x => x.HasKey(p => new { p.UserId, p.StockId }));
        builder.Entity<Portfolio>()
            .HasOne(u => u.User)
            .WithMany(u => u.Portfolios)
            .HasForeignKey(p => p.UserId);
        builder.Entity<Portfolio>()
            .HasOne(u => u.Stock)
            .WithMany(u => u.Portfolios)
            .HasForeignKey(p => p.StockId);

        var roles = new List<IdentityRole>
        {
            new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Name = "User", NormalizedName = "USER" }
        };

        builder.Entity<IdentityRole>().HasData(roles);
    }
    #endregion
}
