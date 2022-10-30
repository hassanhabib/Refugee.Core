using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.ShelterOffers;

namespace RefugeeLand.Core.Api.Services.Foundations.ShelterOffers
{
    public interface IShelterOfferService
    {
        ValueTask<ShelterOffer> AddShelterOfferAsync(ShelterOffer shelterOffer);
    }
}