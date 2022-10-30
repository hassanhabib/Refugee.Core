using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RefugeeLand.Core.Api.Models.ShelterOffers;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<ShelterOffer> ShelterOffers { get; set; }

        public async ValueTask<ShelterOffer> InsertShelterOfferAsync(ShelterOffer shelterOffer) =>
            await InsertAsync(shelterOffer);

        public IQueryable<ShelterOffer> SelectAllShelterOffers()=> SelectAll<ShelterOffer>();

        public async ValueTask<ShelterOffer> SelectShelterOfferByIdAsync(Guid shelterOfferId) =>
            await SelectAsync<ShelterOffer>(shelterOfferId);

        public async ValueTask<ShelterOffer> UpdateShelterOfferAsync(ShelterOffer shelterOffer) =>
            await UpdateAsync(shelterOffer);

        public async ValueTask<ShelterOffer> DeleteShelterOfferAsync(ShelterOffer shelterOffer) =>
            await DeleteAsync(shelterOffer);
    }
}
