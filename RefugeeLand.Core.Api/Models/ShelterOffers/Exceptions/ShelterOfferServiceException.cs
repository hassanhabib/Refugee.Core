// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions
{
    public class ShelterOfferServiceException : Xeption
    {
        public ShelterOfferServiceException(Exception innerException)
            : base(message: "ShelterOffer service error occurred, contact support.", innerException)
        { }
    }
}