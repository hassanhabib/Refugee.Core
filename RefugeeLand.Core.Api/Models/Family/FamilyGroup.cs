using System;
using System.Collections.Generic;
using RefugeeLand.Core.Api.Models.Refugees;

namespace RefugeeLand.Core.Api.Models.Family;

public class FamilyGroup
{
    public Guid Id { get; set; }
    public string FamilyName { get; set; }
    public List<Refugee> Refugee { get; set; }
}