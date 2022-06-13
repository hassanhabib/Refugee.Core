// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;

namespace RefugeeLand.Core.Api.Models.AllowedPets
{
    public class AllowedPet
    {
        public Guid Id { get; set; }
        public PetType Type { get; set; }
    }
}
