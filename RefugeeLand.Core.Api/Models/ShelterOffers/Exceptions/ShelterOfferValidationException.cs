// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions
{
    public class ShelterOfferValidationException : Xeption
    {
        public ShelterOfferValidationException(Xeption innerException)
            : base(message: "ShelterOffer validation errors occurred, please try again.",
                  innerException)
        { }
    }
}