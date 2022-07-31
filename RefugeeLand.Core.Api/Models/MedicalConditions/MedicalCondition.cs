// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using RefugeeLand.Core.Api.Models.PetMedicalConditions;

namespace RefugeeLand.Core.Api.Models.MedicalConditions
{
    public class MedicalCondition
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AdditionalDetails { get; set; }
        
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid UpdatedByUserId { get; set; }
        
        [JsonIgnore]
        public IEnumerable<PetMedicalCondition> PetMedicalConditions { get; set; }
        
       // Todo: RefugeeMedicalCondition 
    }
}