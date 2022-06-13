// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RefugeeLand.Core.Api.Models.RefugeePets;

namespace RefugeeLand.Core.Api.Models.Refugees
{
    public class Refugee
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CurrentLocation { get; set; }
        public string AdditionalDetails { get; set; }
        public bool IsOpenToWork { get; set; }
        public DateTimeOffset BirthDate { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        public RefugeeGender Gender { get; set; } 

        [JsonIgnore] 
        public IEnumerable<RefugeePet> RefugeePets { get; set; }

        // [JsonIgnore] 
        // public IEnumerable<RefugeeLanguage> RefugeeLanguages { get; set; }

        // [JsonIgnore] 
        // public IEnumerable<RefugeeNationality> RefugeeNationalities { get; set; }

        // [JsonIgnore] 
        // public IEnumerable<RefugeeMedicalCondition> RefugeeMedicalConditions { get; set; }

        // [JsonIgnore] 
        // public IEnumerable<RefugeeSkillSet> SkillSets { get; set; }

        // [JsonIgnore] 
        // public IEnumerable<RefugeeContact> RefugeeContacts { get; set; }

        // [JsonIgnore] 
        // public IEnumerable<RefugeeFamilyMembership> RefugeeFamilyMemberships { get; set; }

        // [JsonIgnore]
        // public IEnumerable<RefugeeDocument> RefugeeDocuments { get; set; }
    }
}