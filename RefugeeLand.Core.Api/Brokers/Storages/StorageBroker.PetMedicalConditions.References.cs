// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using RefugeeLand.Core.Api.Models.PetMedicalConditions;
using RefugeeLand.Core.Api.Models.Pets;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetPetMedicalConditionReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PetMedicalCondition>()
                .HasOne(petMedicalCondition => petMedicalCondition.Pet)
                .WithMany(pet => pet.PetMedicalConditions)
                .HasForeignKey(petMedicalCondition => petMedicalCondition.PetId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PetMedicalCondition>()
                .HasOne(petMedicalCondition => petMedicalCondition.MedicalCondition)
                .WithMany(medicalCondition => medicalCondition.PetMedicalConditions)
                .HasForeignKey(refugeePet => refugeePet.MedicalConditionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}