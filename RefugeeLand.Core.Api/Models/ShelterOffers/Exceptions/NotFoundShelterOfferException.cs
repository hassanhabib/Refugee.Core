using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions
{
    public class NotFoundShelterOfferException : Xeption
    {
        public NotFoundShelterOfferException(Guid shelterOfferId)
            : base(message: $"Couldn't find shelterOffer with shelterOfferId: {shelterOfferId}.")
        { }
    }
}