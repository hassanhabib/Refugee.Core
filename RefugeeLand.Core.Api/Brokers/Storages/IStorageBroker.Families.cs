// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.Families;

namespace RefugeeLand.Core.Api.Brokers.Storages;

public partial interface IStorageBroker
{
    ValueTask<Family> InsertFamilyAsync(Family refugee);
    IQueryable<Family> SelectAllFamilies();
    ValueTask<Family> SelectFamilyByIdAsync(Guid refugeeId);
    ValueTask<Family> UpdateFamilyAsync(Family refugee);
    ValueTask<Family> DeleteFamilyAsync(Family refugee);
}