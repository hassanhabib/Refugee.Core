// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using RefugeeLand.Core.Api.Models.Enums;

namespace RefugeeLand.Core.Api.Models.Languages
{
    public class Language
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public FluencyLevel FluencyLevel { get; set; } 
    }
}