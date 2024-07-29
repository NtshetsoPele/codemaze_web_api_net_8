namespace Repository;

public class RepositoryContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder
            .ApplyConfiguration(new CompanyConfiguration())
            .ApplyConfiguration(new EmployeeConfiguration())
            .ApplyConfiguration(new RoleConfiguration());
    }
    
    public DbSet<Company> Companies { get; init; }
    public DbSet<Employee> Employees { get; init; }
}