// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
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
        {
            throw new NotImplementedException();
        }

        public ValueTask<Refugee> SelectRefugeeByIdAsync(Guid refugeeId)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Refugee> UpdateRefugeeAsync(Refugee refugee)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Refugee> DeleteRefugeeAsync(Refugee refugee)
        {
            throw new NotImplementedException();
        }
    }
}