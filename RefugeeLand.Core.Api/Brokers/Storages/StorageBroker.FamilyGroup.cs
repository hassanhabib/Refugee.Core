using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RefugeeLand.Core.Api.Models.Family;

namespace RefugeeLand.Core.Api.Brokers.Storages;

public partial class StorageBroker
{
    private IStorageBroker _storageBrokerImplementation;
    private DbSet<FamilyGroup> FamilyGroups { get; set; }
    
    public async ValueTask<FamilyGroup> InsertFamilyGroupAsync(FamilyGroup refugee)
    {
        using var broker = new StorageBroker(configuration);

        EntityEntry<FamilyGroup> refugeeEntityEntry =
            await broker.AddAsync(refugee);

        await broker.SaveChangesAsync();

        return refugeeEntityEntry.Entity;
    }



    public IQueryable<FamilyGroup> SelectAllFamilyGroups()
    {
        using var broker = new StorageBroker(this.configuration);

        return broker.FamilyGroups;
    }

    public async ValueTask<FamilyGroup> SelectFamilyGroupByIdAsync(Guid refugeeId)
    {
        using var broker = new StorageBroker(configuration);

        return await broker.FamilyGroups.FindAsync(refugeeId);
    }




    public async ValueTask<FamilyGroup> UpdateFamilyGroupAsync(FamilyGroup refugee)
    {
        using var broker = new StorageBroker(configuration);

        EntityEntry<FamilyGroup> refugeeEntityEntry =
            broker.FamilyGroups.Update(refugee);

        await broker.SaveChangesAsync();

        return refugeeEntityEntry.Entity;
    }

    public async ValueTask<FamilyGroup> DeleteFamilyGroupAsync(FamilyGroup refugee)
    {
        using var broker = new StorageBroker(configuration);

        EntityEntry<FamilyGroup> refugeeEntityEntry =
            broker.FamilyGroups.Remove(refugee);

        await broker.SaveChangesAsync();

        return refugeeEntityEntry.Entity;
    }
}