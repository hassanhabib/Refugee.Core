// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

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
            using var broker = new StorageBroker(configuration);

            EntityEntry<Host> hostEntityEntry = 
                await broker.AddAsync(host);

            await broker.SaveChangesAsync();

            return hostEntityEntry.Entity;
        } 
    }
}