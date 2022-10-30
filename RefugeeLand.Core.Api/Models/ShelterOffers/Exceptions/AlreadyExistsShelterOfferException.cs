using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions
{
    public class AlreadyExistsShelterOfferException : Xeption
    {
        public AlreadyExistsShelterOfferException(Exception innerException)
            : base(message: "ShelterOffer with the same Id already exists.", innerException)
        { }
    }
}