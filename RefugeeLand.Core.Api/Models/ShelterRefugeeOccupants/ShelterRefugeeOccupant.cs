// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using RefugeeLand.Core.Api.Models.Refugees;
using RefugeeLand.Core.Api.Models.Shelters;

namespace RefugeeLand.Core.Api.Models.ShelterRefugeeOccupants
{
    public class ShelterRefugeeOccupant
    {
        public Guid Id { get; set; }
        
        public Guid ShelterId { get; set; }
        public Shelter Shelter { get; set; }

        public Guid RefugeeId { get; set; }
        public Refugee Refugee { get; set; }

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        
        public InhabitationStatus InhabitationStatus { get; set; }
        
        // public DateTimeOffset StartDateFromAgreement { get; set; }
        // public DateTimeOffset EndDateFromAgreement { get; set; }
    }
}