using Microsoft.EntityFrameworkCore;
using TaskManager.API.Models;
namespace TaskManager.API
{
    public class TaskDb : DbContext
    {
        public TaskDb(DbContextOptions<TaskDb> options) : base(options)
        {
        }
        public DbSet<TaskItem> Tasks => Set<TaskItem>();

    }
}