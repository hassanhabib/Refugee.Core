// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

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