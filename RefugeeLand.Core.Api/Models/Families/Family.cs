// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Collections.Generic;
using RefugeeLand.Core.Api.Models.Refugees;

namespace RefugeeLand.Core.Api.Models.Families;

public class Family
{
    public Guid Guid { get; set; }
    public string FamilyName { get; set; }
    public List<Refugee>? Refugees { get; set; }
}