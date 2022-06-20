// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using RefugeeLand.Core.Api.Models.AllowedPets;
using RefugeeLand.Core.Api.Models.Hosts;
using RefugeeLand.Core.Api.Models.ShelterOffers;
using RefugeeLand.Core.Api.Models.ShelterRefugeeOccupants;

namespace RefugeeLand.Core.Api.Models.Shelters
{
    public class Shelter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid HostId { get; set; }
        public Host Host { get; set; }

        public string Location { get; set; }
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
        public ShelterStatus Status { get; set; }
        public ShelterType Type { get; set; }
        public ShelterPropertyType PropertyType { get; set; }
        public IEnumerable<AllowedPet> AllowedPets { get; set; }

        DateTimeOffset CreatedDate { get; set; }
        DateTimeOffset UpdatedDate { get; set; }
        Guid CreatedBy { get; set; }
        Guid UpdatedBy { get; set; }

        [JsonIgnore] 
        public IEnumerable<ShelterRefugeeOccupant> ShelterRefugeeOccupants { get; set; }

        [JsonIgnore] 
        public IEnumerable<ShelterOffer> ShelterOffers { get; set; }
        
        // [JsonIgnore] 
        // public IEnumerable<ShelterHostOccupant> ShelterHostOccupants { get; set; }

        // [JsonIgnore]
        // public IEnumerable<AdditionalFamilyMemberOccupant> AdditionalFamilyMemberOccupants { get; set; }

        // [JsonIgnore]
        // public IEnumerable<AdditionalResidentOccupant> AdditionalResidentOccupants { get; set; }
    }
}