// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

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