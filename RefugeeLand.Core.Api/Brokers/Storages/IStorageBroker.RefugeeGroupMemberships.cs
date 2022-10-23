using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.RefugeeGroupMemberships;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<RefugeeGroupMembership> InsertRefugeeGroupMembershipAsync(RefugeeGroupMembership refugeeGroupMembership);
        IQueryable<RefugeeGroupMembership> SelectAllRefugeeGroupMemberships();
    }
}
