using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions
{
    public class FailedShelterOfferStorageException : Xeption
    {
        public FailedShelterOfferStorageException(Exception innerException)
            : base(message: "Failed shelterOffer storage error occurred, contact support.", innerException)
        { }
    }
}