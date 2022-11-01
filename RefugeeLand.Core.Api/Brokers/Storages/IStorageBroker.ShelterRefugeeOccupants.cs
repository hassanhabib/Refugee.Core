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
    }
}
