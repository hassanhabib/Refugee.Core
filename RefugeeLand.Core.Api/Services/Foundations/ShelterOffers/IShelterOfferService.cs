// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.ShelterOffers;

namespace RefugeeLand.Core.Api.Services.Foundations.ShelterOffers
{
    public interface IShelterOfferService
    {
        ValueTask<ShelterOffer> AddShelterOfferAsync(ShelterOffer shelterOffer);
        IQueryable<ShelterOffer> RetrieveAllShelterOffers();
        ValueTask<ShelterOffer> RetrieveShelterOfferByIdAsync(Guid shelterOfferId);
        ValueTask<ShelterOffer> ModifyShelterOfferAsync(ShelterOffer shelterOffer);
        ValueTask<ShelterOffer> RemoveShelterOfferByIdAsync(Guid shelterOfferId);
    }
}