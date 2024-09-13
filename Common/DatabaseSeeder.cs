using Microsoft.EntityFrameworkCore;

namespace ContosoOnline.Common;

public abstract class DatabaseSeeder<T>(Action<T>? seedAction = null)
    where T : DbContext
{
    public abstract Task Seed(T dbContext);
}
