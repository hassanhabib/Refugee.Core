// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Collections.Generic;
using RefugeeLand.Core.Api.Models.Refugees;

namespace RefugeeLand.Core.Api.Models.RefugeeGroups;

public class RefugeeGroup
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IList<Refugee> Refugee { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }
}