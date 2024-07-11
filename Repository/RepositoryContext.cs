namespace Repository;

public class RepositoryContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CompanyConfiguration()); 
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
    }
    
    public DbSet<Company> Companies { get; init; }
    public DbSet<Employee> Employees { get; init; }
}