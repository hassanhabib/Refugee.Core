// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions
{
    public class LockedShelterOfferException : Xeption
    {
        public LockedShelterOfferException(Exception innerException)
            : base(message: "Locked shelterOffer record exception, please try again later", innerException)
        {
        }
    }
}