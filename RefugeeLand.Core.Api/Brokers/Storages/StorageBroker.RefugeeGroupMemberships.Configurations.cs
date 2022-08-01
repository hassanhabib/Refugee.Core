// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------


using Microsoft.EntityFrameworkCore;
using RefugeeLand.Core.Api.Models.RefugeeGroupMemberships;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetRefugeeGroupMembershipReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefugeeGroupMembership>()
                .HasOne(refugeeGroupMembership => refugeeGroupMembership.Refugee)
                .WithMany(refugee => refugee.RefugeeGroupMemberships)
                .HasForeignKey(refugeeGroupMembership => refugeeGroupMembership.RefugeeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RefugeeGroupMembership>()
                .HasOne(refugeeGroupMembership => refugeeGroupMembership.RefugeeGroup)
                .WithMany(refugeeGroup => refugeeGroup.RefugeeGroupMemberships)
                .HasForeignKey(refugeeGroupMembership => refugeeGroupMembership.RefugeeGroupId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}