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
        public AllowedPetType Type { get; set; }
        
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid UpdatedByUserId { get; set; }
        
    }
}
