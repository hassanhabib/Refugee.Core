using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.ShelterOffers;
using RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions;
using Xeptions;

namespace RefugeeLand.Core.Api.Services.Foundations.ShelterOffers
{
    public partial class ShelterOfferService
    {
        private delegate ValueTask<ShelterOffer> ReturningShelterOfferFunction();

        private async ValueTask<ShelterOffer> TryCatch(ReturningShelterOfferFunction returningShelterOfferFunction)
        {
            try
            {
                return await returningShelterOfferFunction();
            }
            catch (NullShelterOfferException nullShelterOfferException)
            {
                throw CreateAndLogValidationException(nullShelterOfferException);
            }
        }

        private ShelterOfferValidationException CreateAndLogValidationException(Xeption exception)
        {
            var shelterOfferValidationException =
                new ShelterOfferValidationException(exception);

            this.loggingBroker.LogError(shelterOfferValidationException);

            return shelterOfferValidationException;
        }
    }
}