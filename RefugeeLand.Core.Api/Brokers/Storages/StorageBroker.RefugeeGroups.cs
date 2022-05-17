// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using RefugeeLand.Core.Api.Models.RefugeeGroups;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<RefugeeGroup> RefugeeGroups { get; set; }
    }
}