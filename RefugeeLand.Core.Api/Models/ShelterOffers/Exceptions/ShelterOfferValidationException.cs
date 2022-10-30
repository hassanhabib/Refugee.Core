using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions
{
    public class ShelterOfferValidationException : Xeption
    {
        public ShelterOfferValidationException(Xeption innerException)
            : base(message: "ShelterOffer validation errors occurred, please try again.",
                  innerException)
        { }
    }
}