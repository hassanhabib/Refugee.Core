using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RefugeeLand.Core.Api.Models.Hosts;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Host> Hosts { get; set; }

        public async ValueTask<Host> InsertHostAsync(Host host)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Host> hostEntityEntry =
                await broker.Hosts.AddAsync(host);

            await broker.SaveChangesAsync();

            return hostEntityEntry.Entity;
        }

        public IQueryable<Host> SelectAllHosts()
        {
            using var broker =
                new StorageBroker(this.configuration);

            return broker.Hosts;
        }

        public async ValueTask<Host> SelectHostByIdAsync(Guid hostId)
        {
            using var broker =
                new StorageBroker(this.configuration);

            return await broker.Hosts.FindAsync(hostId);
        }
    }
}
