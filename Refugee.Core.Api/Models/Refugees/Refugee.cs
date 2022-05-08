// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Refugee.Core.Api.Models.Enums;
using System;
using System.Collections.Generic;

namespace Refugee.Core.Api.Models.Refugees
{
    public class Refugee
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CurrentLocation { get; set; }
        public Gender Gender { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public IEnumerable<string> MedicalConditions { get; set; }
        public IEnumerable<string> SkillSets { get; set; }
        public IEnumerable<string> Languages { get; set; }
        public bool IsOpenToWork { get; set; }
        public string AdditionalDetails { get; set; }
        //todo: Replace with Contact information class
        public string Phone { get; set; }
        public string Email { get; set; }
        //todo: Add Family class or RefugeeGroup
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}