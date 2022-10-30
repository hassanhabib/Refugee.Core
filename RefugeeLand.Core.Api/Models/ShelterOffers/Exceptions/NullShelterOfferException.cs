using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions
{
    public class NullShelterOfferException : Xeption
    {
        public NullShelterOfferException()
            : base(message: "ShelterOffer is null.")
        { }
    }
}