// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

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