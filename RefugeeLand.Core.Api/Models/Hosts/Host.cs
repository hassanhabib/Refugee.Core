// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using RefugeeLand.Core.Api.Models.ShelterOffers;
using RefugeeLand.Core.Api.Models.Shelters;

namespace RefugeeLand.Core.Api.Models.Hosts
{
    public class Host
    {
        public Guid Id { get; set; }
        // public string FirstName { get; set; } Todo: uncomment those properties after standardly foundation service setup
        // public string MiddleName { get; set; }
        // public string LastName { get; set; }
        // public string AdditionalDetails { get; set; }
        // public HostGender Gender { get; set; }
        // public DateTimeOffset BirthDate { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        
        [JsonIgnore] 
        public IEnumerable<Shelter> Shelters { get; set; }
        
        [JsonIgnore] 
        public IEnumerable<ShelterOffer> ShelterOffers { get; set; }
        
        // [JsonIgnore] 
        // public IEnumerable<HostContact> HostContacts { get; set; }

        // [JsonIgnore] 
        // public IEnumerable<HostFamilyMembership> HostFamilyMemberships { get; set; }

        // [JsonIgnore] 
        // public IEnumerable<HostLanguage> HostLanguages { get; set; }

        // [JsonIgnore] 
        // public IEnumerable<HostNationality> HostNationalities { get; set; }

        // [JsonIgnore] 
        // public IEnumerable<HostMedicalCondition> HostMedicalConditions { get; set; }

        // [JsonIgnore] 
        // public IEnumerable<HostDocument> HostDocuments { get; set; }
    }
}
