using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RefugeeLand.Core.Api.Models.ShelterRequests;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<ShelterRequest> ShelterRequests { get; set; }

        public async ValueTask<ShelterRequest> InsertShelterRequestAsync(ShelterRequest shelterRequest) =>
            await InsertAsync(shelterRequest);
    }
}
