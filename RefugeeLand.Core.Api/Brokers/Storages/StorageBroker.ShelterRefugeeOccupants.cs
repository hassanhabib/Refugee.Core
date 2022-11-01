using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RefugeeLand.Core.Api.Models.ShelterRefugeeOccupants;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<ShelterRefugeeOccupant> ShelterRefugeeOccupants { get; set; }

        public async ValueTask<ShelterRefugeeOccupant> InsertShelterRefugeeOccupantAsync(ShelterRefugeeOccupant shelterRefugeeOccupant) =>
            await InsertAsync(shelterRefugeeOccupant);
    }
}
