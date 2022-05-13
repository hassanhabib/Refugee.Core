// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.Families;

namespace RefugeeLand.Core.Api.Brokers.Storages;

public partial class StorageBroker
{
    public ValueTask<Family> InsertFamilyAsync(Family refugee)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Family> SelectAllFamilies()
    {
        throw new NotImplementedException();
    }

    public ValueTask<Family> SelectFamilyByIdAsync(Guid refugeeId)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Family> UpdateFamilyAsync(Family refugee)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Family> DeleteFamilyAsync(Family refugee)
    {
        throw new NotImplementedException();
    }
}