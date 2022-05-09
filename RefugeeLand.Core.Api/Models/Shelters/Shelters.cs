// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using RefugeeLand.Core.Api.Models.Enums;

namespace RefugeeLand.Core.Api.Models.Shelters
{
    public class Shelters
    {
        public Guid Id { get; set; }
        public Guid HostId { get; set; }
        public string ShelterType { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string AdditionalDetails { get; set; }
        public int MaximumCapacity { get; set; }
        public int BedroomNumber { get; set; }
        public int SingleBedNumber { get; set; }
        public int DoubleBedNumber { get; set; }
        public int ChildBedNumber { get; set; }
        public int BabyBedNumber { get; set; }
        public bool IsSmokingAllowed { get; set; }
        public bool IsPetAllowed { get; set; }
        public bool IsHandicappedAccessible { get; set; }
        public bool IsVerified { get; set; }
        public bool IsShared { get; set; }
        public IEnumerable<Pets> AllowedPets { get; set; }
        
    }
}