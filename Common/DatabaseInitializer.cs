using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ContosoOnline.Common;

public class DatabaseInitializer<T>(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
    where T : DbContext
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<T>();

        await EnsureDatabaseAsync(dbContext, cancellationToken);
    }

    private static async Task EnsureDatabaseAsync(T dbContext, CancellationToken cancellationToken)
    {
        await dbContext.Database.EnsureCreatedAsync();
    }
}