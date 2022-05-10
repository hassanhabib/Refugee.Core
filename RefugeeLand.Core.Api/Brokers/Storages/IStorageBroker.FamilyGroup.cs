// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.Family;
using RefugeeLand.Core.Api.Models.Refugees;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<FamilyGroup> InsertFamilyGroupAsync(FamilyGroup refugee);
        IQueryable<FamilyGroup> SelectAllFamilyGroups();
        ValueTask<FamilyGroup> SelectFamilyGroupByIdAsync(Guid refugeeId);
        ValueTask<FamilyGroup> UpdateFamilyGroupAsync(FamilyGroup refugee);
        ValueTask<FamilyGroup> DeleteFamilyGroupAsync(FamilyGroup refugee);
    }
}
