using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions
{
    public class FailedShelterOfferServiceException : Xeption
    {
        public FailedShelterOfferServiceException(Exception innerException)
            : base(message: "Failed shelterOffer service occurred, please contact support", innerException)
        { }
    }
}