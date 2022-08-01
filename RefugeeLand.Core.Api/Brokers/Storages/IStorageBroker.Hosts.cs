using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.Hosts;

namespace RefugeeLand.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Host> InsertHostAsync(Host host);
        IQueryable<Host> SelectAllHosts();
        ValueTask<Host> SelectHostByIdAsync(Guid hostId);
        ValueTask<Host> UpdateHostAsync(Host host);
        ValueTask<Host> DeleteHostAsync(Host host);
    }
}
