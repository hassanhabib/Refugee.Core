// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.Shelters;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Shelter> InsertShelterAsync(Shelter shelter);
        IQueryable<Shelter> SelectAllShelters();
        ValueTask<Shelter> SelectShelterByIdAsync(Guid shelterId);
        ValueTask<Shelter> UpdateShelterAsync(Shelter shelter);
    }
}