// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------


using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RefugeeLand.Core.Api.Models.RefugeePets;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<RefugeePet> RefugeePets { get; set; }

        public async ValueTask<RefugeePet> InsertRefugeeAsync(RefugeePet refugeePet)
        {
            using var broker = new StorageBroker(configuration);
            
            EntityEntry<RefugeePet> refugeePetEntity = 
                await broker.AddAsync(refugeePet);

            await broker.SaveChangesAsync();
            
            return refugeePetEntity.Entity ;
        }
    }
}