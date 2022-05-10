using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RefugeeLand.Core.Api.Models.Family;
using RefugeeLand.Core.Api.Models.Refugees;

namespace RefugeeLand.Core.Api.Models.Configuration;

public class FamilyGroupConfiguration:IEntityTypeConfiguration<FamilyGroup>
{
    public void Configure(EntityTypeBuilder<FamilyGroup> builder)
    {
        builder.OwnsMany<Refugee>(t => t.Refugee).WithOwner(t => t.FamilyGroup);
        builder.Property(p => p.FamilyName).IsRequired();
    }
}