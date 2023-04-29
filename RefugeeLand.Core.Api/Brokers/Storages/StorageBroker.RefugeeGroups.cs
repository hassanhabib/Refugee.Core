// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Linq;
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

        public IQueryable<RefugeeGroup> SelectAllRefugeeGroups()
        {
            using var broker =
                new StorageBroker(this.configuration);

            return broker.RefugeeGroups;
        }

        public async ValueTask<RefugeeGroup> SelectRefugeeGroupByIdAsync(Guid refugeeGroupId)
        {
            using var broker =
                new StorageBroker(this.configuration);

            return await broker.RefugeeGroups.FindAsync(refugeeGroupId);
        }
        
        public async ValueTask<RefugeeGroup> UpdateRefugeeGroupAsync(RefugeeGroup refugeeGroup) =>
            await UpdateAsync(refugeeGroup);
    }
}