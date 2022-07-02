// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RefugeeLand.Core.Api.Models.RefugeeGroups;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<RefugeeGroup> RefugeeGroups { get; set; }

        public async ValueTask<RefugeeGroup> InsertRefugeeGroupAsync(RefugeeGroup refugeeGroup)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<RefugeeGroup> refugeeGroupEntityEntry =
                await broker.RefugeeGroups.AddAsync(refugeeGroup);

            await broker.SaveChangesAsync();

            return refugeeGroupEntityEntry.Entity;
        }
    }
}