// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using RefugeeLand.Core.Api.Models.PetMedicalConditions;
using RefugeeLand.Core.Api.Models.RefugeePets;

namespace RefugeeLand.Core.Api.Models.Pets
{
    public class Pet
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public PetType Type { get; set; }
        public PetGender Gender { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string AdditionalDetails { get; set; }
        
        [JsonIgnore]
        public IEnumerable<PetMedicalCondition> PetMedicalConditions { get; set; }
        
        [JsonIgnore]
        public IEnumerable<RefugeePet> RefugeePets { get; set; }
    }
}