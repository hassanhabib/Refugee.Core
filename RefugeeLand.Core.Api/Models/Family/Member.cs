using System;

namespace RefugeeLand.Core.Api.Models.Family;

public class Member
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public int Location { get; set; }
}