// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------


using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RefugeeLand.Core.Api.Models.RefugeeGroupMemberships;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<RefugeeGroupMembership> RefugeeGroupMemberships { get; set; }

        public async ValueTask<RefugeeGroupMembership> InsertRefugeeGroupMembershipAsync(
            RefugeeGroupMembership refugeeGroupMembership)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<RefugeeGroupMembership> refugeeGroupMembershipEntityEntry =
                await broker.AddAsync(refugeeGroupMembership);

            await broker.SaveChangesAsync();

            return refugeeGroupMembershipEntityEntry.Entity;
        }
    }
}