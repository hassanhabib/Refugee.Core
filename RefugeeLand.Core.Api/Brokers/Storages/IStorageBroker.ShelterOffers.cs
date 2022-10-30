using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.ShelterOffers;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<ShelterOffer> InsertShelterOfferAsync(ShelterOffer shelterOffer);
        IQueryable<ShelterOffer> SelectAllShelterOffers();
        ValueTask<ShelterOffer> SelectShelterOfferByIdAsync(Guid shelterOfferId);
    }
}
