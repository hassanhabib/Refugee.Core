// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions
{
    public class InvalidShelterOfferException : Xeption
    {
        public InvalidShelterOfferException()
            : base(message: "Invalid shelterOffer. Please correct the errors and try again.")
        { }
    }
}