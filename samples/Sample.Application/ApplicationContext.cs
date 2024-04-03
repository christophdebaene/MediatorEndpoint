using Microsoft.EntityFrameworkCore;
using Sample.Application.Tasks.Types;

namespace Sample.Application;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<Document> Documents { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
}
