using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RefugeeLand.Core.Api.Models.RefugeeGroupMemberships;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<RefugeeGroupMembership> RefugeeGroupMemberships { get; set; }

        public async ValueTask<RefugeeGroupMembership> InsertRefugeeGroupMembershipAsync(RefugeeGroupMembership refugeeGroupMembership)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<RefugeeGroupMembership> refugeeGroupMembershipEntityEntry =
                await broker.RefugeeGroupMemberships.AddAsync(refugeeGroupMembership);

            await broker.SaveChangesAsync();

            return refugeeGroupMembershipEntityEntry.Entity;
        }

        public IQueryable<RefugeeGroupMembership> SelectAllRefugeeGroupMemberships()
        {
            using var broker =
                new StorageBroker(this.configuration);

            return broker.RefugeeGroupMemberships;
        }

        public async ValueTask<RefugeeGroupMembership> SelectRefugeeGroupMembershipByIdAsync(Guid refugeeGroupMembershipId)
        {
            using var broker =
                new StorageBroker(this.configuration);

            return await broker.RefugeeGroupMemberships.FindAsync(refugeeGroupMembershipId);
        }

        public async ValueTask<RefugeeGroupMembership> UpdateRefugeeGroupMembershipAsync(RefugeeGroupMembership refugeeGroupMembership)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<RefugeeGroupMembership> refugeeGroupMembershipEntityEntry =
                broker.RefugeeGroupMemberships.Update(refugeeGroupMembership);

            await broker.SaveChangesAsync();

            return refugeeGroupMembershipEntityEntry.Entity;
        }

        public async ValueTask<RefugeeGroupMembership> DeleteRefugeeGroupMembershipAsync(RefugeeGroupMembership refugeeGroupMembership)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<RefugeeGroupMembership> refugeeGroupMembershipEntityEntry =
                broker.RefugeeGroupMemberships.Remove(refugeeGroupMembership);

            await broker.SaveChangesAsync();

            return refugeeGroupMembershipEntityEntry.Entity;
        }
    }
}
