using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions
{
    public class ShelterOfferDependencyException : Xeption
    {
        public ShelterOfferDependencyException(Xeption innerException) :
            base(message: "ShelterOffer dependency error occurred, contact support.", innerException)
        { }
    }
}