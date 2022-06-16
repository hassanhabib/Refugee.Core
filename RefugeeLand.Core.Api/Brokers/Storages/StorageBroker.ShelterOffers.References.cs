// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using RefugeeLand.Core.Api.Models.ShelterOffers;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetShelterOfferReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShelterOffer>()
                .HasOne(shelterOffer => shelterOffer.Shelter)
                .WithMany(shelter => shelter.ShelterOffers)
                .HasForeignKey(shelterOffer => shelterOffer.ShelterId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<ShelterOffer>()
                .HasOne(shelterOffer => shelterOffer.Host)
                .WithMany(host => host.ShelterOffers)
                .HasForeignKey(shelterOffer => shelterOffer.HostId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}