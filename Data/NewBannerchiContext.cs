using Microsoft.EntityFrameworkCore;
using NewBannerchi.Entities;

namespace NewBannerchi.Data;

public class NewBannerchiContext:DbContext
{
    public NewBannerchiContext(DbContextOptions<NewBannerchiContext> options):base(options){

    }
    
    public DbSet<Package> Packages  => Set<Package>();
    
    public DbSet<User> Users  => Set<User>();
    
    public DbSet<Category> Categories  => Set<Category>();
    
    public DbSet<Occasion> Occasions  => Set<Occasion>();

    public DbSet<UserOtp> UsersOtp => Set<UserOtp>();

    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }*/
}
