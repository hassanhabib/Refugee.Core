// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------


using Microsoft.EntityFrameworkCore;
using RefugeeLand.Core.Api.Models.ShelterRefugeeOccupants;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetShelterRefugeeOccupantReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShelterRefugeeOccupant>()
                .HasKey(shelterRefugeeOccupant =>
                    new { shelterRefugeeOccupant.ShelterId, shelterRefugeeOccupant.RefugeeId });

            modelBuilder.Entity<ShelterRefugeeOccupant>()
                .HasOne(shelterRefugeeOccupant => shelterRefugeeOccupant.Shelter)
                .WithMany(shelter => shelter.ShelterRefugeeOccupants)
                .HasForeignKey(shelterRefugeeOccupant => shelterRefugeeOccupant.ShelterId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<ShelterRefugeeOccupant>()
                .HasOne(shelterRefugeeOccupant => shelterRefugeeOccupant.Refugee)
                .WithMany(refugee => refugee.ShelterRefugeeOccupants)
                .HasForeignKey(shelterRefugeeOccupant => shelterRefugeeOccupant.RefugeeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}