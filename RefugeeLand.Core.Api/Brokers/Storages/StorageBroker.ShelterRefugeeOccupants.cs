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

        public IQueryable<ShelterRefugeeOccupant> SelectAllShelterRefugeeOccupants()=> SelectAll<ShelterRefugeeOccupant>();

        public async ValueTask<ShelterRefugeeOccupant> SelectShelterRefugeeOccupantByIdAsync(Guid shelterRefugeeOccupantId) =>
            await SelectAsync<ShelterRefugeeOccupant>(shelterRefugeeOccupantId);

        public async ValueTask<ShelterRefugeeOccupant> UpdateShelterRefugeeOccupantAsync(ShelterRefugeeOccupant shelterRefugeeOccupant) =>
            await UpdateAsync(shelterRefugeeOccupant);

        public async ValueTask<ShelterRefugeeOccupant> DeleteShelterRefugeeOccupantAsync(ShelterRefugeeOccupant shelterRefugeeOccupant) =>
            await DeleteAsync(shelterRefugeeOccupant);
    }
}
