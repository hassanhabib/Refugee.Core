// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using RefugeeLand.Core.Api.Models.Pets;
using RefugeeLand.Core.Api.Models.Refugees;

namespace RefugeeLand.Core.Api.Models.RefugeePets
{
    public class RefugeePet
    {
        public Guid RefugeeId { get; set; }
        public Refugee Refugee { get; set; }

        public Guid PetId { get; set; }
        public Pet Pet { get; set; }
    }
}