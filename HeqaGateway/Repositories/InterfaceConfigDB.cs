using Microsoft.EntityFrameworkCore;
 
public class InterfaceConfigDB : DbContext
{
    public InterfaceConfigDB(DbContextOptions<InterfaceConfigDB> options)
        : base(options) { }

    public DbSet<InterfaceConfig> interfaceConfig => Set<InterfaceConfig>();
}
