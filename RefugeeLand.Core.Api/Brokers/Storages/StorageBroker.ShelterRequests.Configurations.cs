// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using RefugeeLand.Core.Api.Models.ShelterRequests;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void AddShelterRequestConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShelterRequest>()
                .HasOne(shelterRequest => shelterRequest.ShelterOffer)
                .WithMany(shelterOffer => shelterOffer.ShelterRequests)
                .HasForeignKey(shelterRequest => shelterRequest.ShelterOfferId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<ShelterRequest>()
                .HasOne(shelterRequest => shelterRequest.RefugeeGroup)
                .WithMany(refugeeGroup => refugeeGroup.ShelterRequests)
                .HasForeignKey(shelterRequest => shelterRequest.RefugeeGroupId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<ShelterRequest>()
                .HasOne(shelterRequest => shelterRequest.RefugeeApplicant)
                .WithMany(refugee => refugee.ShelterRequests)
                .HasForeignKey(shelterRequest => shelterRequest.RefugeeApplicantId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
