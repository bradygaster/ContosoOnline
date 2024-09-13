using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ContosoOnline.Common;

public class DatabaseInitializer<T>(IServiceProvider serviceProvider, DatabaseSeeder<T>? seeder = null)
    : BackgroundService where T : DbContext
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<T>();

        await EnsureDatabaseAsync(dbContext, cancellationToken);

        if(seeder is not null)
        {
            await seeder.Seed(dbContext);
        }
    }

    private static async Task EnsureDatabaseAsync(T dbContext, CancellationToken cancellationToken)
    {
        await dbContext.Database.EnsureCreatedAsync();
    }
}