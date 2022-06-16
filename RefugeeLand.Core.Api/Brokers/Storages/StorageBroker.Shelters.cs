// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------


using Microsoft.EntityFrameworkCore;
using RefugeeLand.Core.Api.Models.Shelters;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Shelter> Shelters { get; set; }
    }
}