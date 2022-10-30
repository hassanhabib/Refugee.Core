// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------


using Microsoft.EntityFrameworkCore;
using RefugeeLand.Core.Api.Models.RefugeePets;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void AddRefugeePetConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefugeePet>()
                .HasOne(refugeePet => refugeePet.Pet)
                .WithMany(pet => pet.RefugeePets)
                .HasForeignKey(refugeePet => refugeePet.PetId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RefugeePet>()
                .HasOne(refugeePet => refugeePet.Refugee)
                .WithMany(refugee => refugee.RefugeePets)
                .HasForeignKey(refugeePet => refugeePet.RefugeeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}