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
    }
}
