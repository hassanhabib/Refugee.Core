// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RefugeeLand.Core.Api.Models.Nationalities;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Nationality> Nationalities { get; set; }

        public async ValueTask<Nationality> InsertNationalityAsync(Nationality nationality)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Nationality> nationalityEntityEntry =
                await broker.Nationalities.AddAsync(nationality);

            await broker.SaveChangesAsync();

            return nationalityEntityEntry.Entity;
        }

        public IQueryable<Nationality> SelectAllNationalities()
        {
            using var broker =
                new StorageBroker(this.configuration);

            return broker.Nationalities;
        }

        public async ValueTask<Nationality> SelectNationalityByIdAsync(Guid nationalityId)
        {
            using var broker =
                new StorageBroker(this.configuration);

            return await broker.Nationalities.FindAsync(nationalityId);
        }

        public async ValueTask<Nationality> UpdateNationalityAsync(Nationality nationality)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Nationality> nationalityEntityEntry =
                broker.Nationalities.Update(nationality);

            await broker.SaveChangesAsync();

            return nationalityEntityEntry.Entity;
        }

        public async ValueTask<Nationality> DeleteNationalityAsync(Nationality nationality)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Nationality> nationalityEntityEntry =
                broker.Nationalities.Remove(nationality);

            await broker.SaveChangesAsync();

            return nationalityEntityEntry.Entity;
        }
    }
}
