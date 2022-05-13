// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------


using System;

namespace RefugeeLand.Core.Api.Models.ContactInformation;

// can be reused
public class ContactInformation
{
    public Guid Guid { get; set; }
    // full qualified ISO E.123 phone number
    public string Phone { get; set; }
    public string Email { get; set; }
}