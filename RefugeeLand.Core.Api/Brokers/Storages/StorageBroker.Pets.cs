using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RefugeeLand.Core.Api.Models.Pets;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Pet> Pets { get; set; }

        public async ValueTask<Pet> InsertPetAsync(Pet pet) =>
            await InsertAsync(pet);

        public IQueryable<Pet> SelectAllPets()=> SelectAll<Pet>();

        public async ValueTask<Pet> SelectPetByIdAsync(Guid petId) =>
            await SelectAsync<Pet>(petId);

        public async ValueTask<Pet> UpdatePetAsync(Pet pet) =>
            await UpdateAsync(pet);

        public async ValueTask<Pet> DeletePetAsync(Pet pet) =>
            await DeleteAsync(pet);
    }
}
