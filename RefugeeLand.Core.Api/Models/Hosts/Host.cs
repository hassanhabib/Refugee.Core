// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Collections.Generic;
using RefugeeLand.Core.Api.Models.Enums;

namespace RefugeeLand.Core.Api.Models.Hosts
{
    public class Host
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public IEnumerable<string> Languages { get; set; }
        public IEnumerable<string> Nationalities { get; set; }
        public string AdditionalDetails { get; set; }
        //todo: Replace with Contact information class
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        //todo: Add List of Shelters
    }
}