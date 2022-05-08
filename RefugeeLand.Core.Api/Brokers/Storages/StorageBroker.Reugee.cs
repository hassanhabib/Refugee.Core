// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RefugeeLand.Core.Api.Models.Refugees;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Refugee> Refugees { get; set; }

        public async ValueTask<Refugee> InsertRefugeeAsync(Refugee refugee)
        {
            using var broker = new StorageBroker(configuration);

            EntityEntry<Refugee> refugeeEntityEntry = await broker.AddAsync(refugee);

            await broker.SaveChangesAsync();

            return refugeeEntityEntry.Entity;
        }

        public IQueryable<Refugee> SelectAllRefugees()
            => this.Refugees;

        public async ValueTask<Refugee> SelectRefugeeByIdAsync(Guid refugeeId)
        {
            using var broker = new StorageBroker(configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.Refugees.FindAsync(refugeeId);
        }

        public async ValueTask<Refugee> UpdateRefugeeAsync(Refugee refugee)
        {
            using var broker = new StorageBroker(configuration);

            EntityEntry<Refugee> refugeeEntityEntry = broker.Refugees.Update(refugee);

            await broker.SaveChangesAsync();

            return refugeeEntityEntry.Entity;
        }

        public async ValueTask<Refugee> DeleteRefugeeAsync(Refugee refugee)
        {
            using var broker = new StorageBroker(configuration);

            EntityEntry<Refugee> refugeeEntityEntry = broker.Refugees.Remove(refugee);

            await broker.SaveChangesAsync();

            return refugeeEntityEntry.Entity;
        }
    }
}