// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.RefugeeGroupMemberships;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<RefugeeGroupMembership> InsertRefugeeGroupMembershipAsync(
            RefugeeGroupMembership refugeeGroupMembership);
    }
}