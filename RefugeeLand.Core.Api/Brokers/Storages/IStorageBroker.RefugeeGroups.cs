// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.RefugeeGroups;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<RefugeeGroup> InsertRefugeeGroupAsync(RefugeeGroup refugeeGroup);
        IQueryable<RefugeeGroup> SelectAllRefugeeGroups();
    }
}