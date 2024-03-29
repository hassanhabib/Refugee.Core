// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.ShelterRequests;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<ShelterRequest> InsertShelterRequestAsync(ShelterRequest shelterRequest);
        IQueryable<ShelterRequest> SelectAllShelterRequests();
        ValueTask<ShelterRequest> SelectShelterRequestByIdAsync(Guid shelterRequestId);
        ValueTask<ShelterRequest> UpdateShelterRequestAsync(ShelterRequest shelterRequest);
        ValueTask<ShelterRequest> DeleteShelterRequestAsync(ShelterRequest shelterRequest);
    }
}
