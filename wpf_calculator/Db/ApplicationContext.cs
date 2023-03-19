using Microsoft.EntityFrameworkCore;
using wpf_calculator.Models.Entity;

namespace wpf_calculator.History.Db;

public class ApplicationContext : DbContext
{
    public DbSet<HistoryLogEntity> HistoryLogs { get; set; } = null!;
    public DbSet<AppLogEntity> AppLogs { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=calcapp.db");
    }
}