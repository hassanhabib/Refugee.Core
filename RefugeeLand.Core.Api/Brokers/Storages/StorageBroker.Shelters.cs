﻿// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------


using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RefugeeLand.Core.Api.Models.Shelters;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Shelter> Shelters { get; set; }

        public async ValueTask<Shelter> InsertShelterAsync(Shelter shelter)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<Shelter> shelterEntityEntry =
                await broker.AddAsync(shelter);

            await broker.SaveChangesAsync();

            return shelterEntityEntry.Entity;
        }
    }
}