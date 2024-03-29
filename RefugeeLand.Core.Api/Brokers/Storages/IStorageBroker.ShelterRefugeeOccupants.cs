﻿// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.ShelterRefugeeOccupants;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<ShelterRefugeeOccupant> InsertShelterRefugeeOccupantAsync(ShelterRefugeeOccupant shelterRefugeeOccupant);
        IQueryable<ShelterRefugeeOccupant> SelectAllShelterRefugeeOccupants();
        ValueTask<ShelterRefugeeOccupant> SelectShelterRefugeeOccupantByIdAsync(Guid shelterRefugeeOccupantId);
        ValueTask<ShelterRefugeeOccupant> UpdateShelterRefugeeOccupantAsync(ShelterRefugeeOccupant shelterRefugeeOccupant);
        ValueTask<ShelterRefugeeOccupant> DeleteShelterRefugeeOccupantAsync(ShelterRefugeeOccupant shelterRefugeeOccupant);
    }
}
