using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions
{
    public class ShelterOfferDependencyValidationException : Xeption
    {
        public ShelterOfferDependencyValidationException(Xeption innerException)
            : base(message: "ShelterOffer dependency validation occurred, please try again.", innerException)
        { }
    }
}