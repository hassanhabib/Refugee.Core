// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Refugee.Core.Api.Models.Refugees
{
    public class Refugee
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string CurrentLocation { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AdditionalDetails { get; set; }
        public IList<string> Languages { get; set; }
        public IList<string> Nationalities { get; set; }
        public IList<string> FamilyMembers { get; set; }
        public IList<string> Pets { get; set; }
        public IList<string> Properties { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public GenderType Gender { get; set; }

    }

    public enum GenderType 
    { MALE = 0, 
      FEMALE = 1, 
      OTHER = 2,
    }
}
