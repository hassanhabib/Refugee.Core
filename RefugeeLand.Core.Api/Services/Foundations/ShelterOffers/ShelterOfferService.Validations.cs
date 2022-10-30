using RefugeeLand.Core.Api.Models.ShelterOffers;
using RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions;

namespace RefugeeLand.Core.Api.Services.Foundations.ShelterOffers
{
    public partial class ShelterOfferService
    {
        private void ValidateShelterOfferOnAdd(ShelterOffer shelterOffer)
        {
            ValidateShelterOfferIsNotNull(shelterOffer);
        }

        private static void ValidateShelterOfferIsNotNull(ShelterOffer shelterOffer)
        {
            if (shelterOffer is null)
            {
                throw new NullShelterOfferException();
            }
        }
    }
}