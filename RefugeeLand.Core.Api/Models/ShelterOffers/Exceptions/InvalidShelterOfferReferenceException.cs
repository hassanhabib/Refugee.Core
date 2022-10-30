using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions
{
    public class InvalidShelterOfferReferenceException : Xeption
    {
        public InvalidShelterOfferReferenceException(Exception innerException)
            : base(message: "Invalid shelterOffer reference error occurred.", innerException) { }
    }
}