namespace Entities.Models;

public sealed class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Name = "Manager", 
                NormalizedName = "MANAGER"
            },
            new IdentityRole
            {
                Name = "Administrator", 
                NormalizedName = "ADMINISTRATOR"
            }
        );
    }
}