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
    }
}
