// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------


using System;
using RefugeeLand.Core.Api.Models.Refugees;

namespace RefugeeLand.Core.Api.Models.ContactInformations;

// can be reused
public class RefugeeContact
{
    public Guid Guid { get; set; }

    public Refugee Refugee { get; set; }
    // full qualified ISO E.123 phone number
    public string Phone { get; set; }
    public string Email { get; set; }
}