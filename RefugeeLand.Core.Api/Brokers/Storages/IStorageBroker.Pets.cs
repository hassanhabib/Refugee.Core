using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.Pets;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Pet> InsertPetAsync(Pet pet);
        IQueryable<Pet> SelectAllPets();
        ValueTask<Pet> SelectPetByIdAsync(Guid petId);
    }
}
