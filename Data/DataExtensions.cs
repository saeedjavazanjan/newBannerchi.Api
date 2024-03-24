using Microsoft.EntityFrameworkCore;
using NewBannerchi.Authentication;
using NewBannerchi.Repository;

namespace NewBannerchi.Data;

public static class DataExtensions
{
    public static async Task InitializeDbAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<NewBannerchiContext>();
        await dbContext.Database.MigrateAsync();
    }

    public static IServiceCollection AddRepositories(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connString=configuration.GetConnectionString("NewBannerchiContext");
        services.AddSqlServer<NewBannerchiContext>(connString)
            .AddScoped<IRepository,EntityFrameWorkRepository>();
        return services;

    }


    
    public static IServiceCollection AddJwtProvider(
        this IServiceCollection services
    )
    {
        services.AddScoped<IJwtProvider, JwtProvider>();
        return services;
    }

    /*public static IServiceCollection AddFileService(
        this IServiceCollection services
    )
    {
        services.AddScoped<IFileService, FileService>();
        return services;
    }*/
    

    
    
}