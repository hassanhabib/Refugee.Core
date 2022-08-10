using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.Hosts;

namespace RefugeeLand.Core.Api.Services.Foundations.Hosts
{
    public interface IHostService
    {
        ValueTask<Host> AddHostAsync(Host host);
        IQueryable<Host> RetrieveAllHosts();
        ValueTask<Host> RetrieveHostByIdAsync(Guid hostId);
        ValueTask<Host> ModifyHostAsync(Host host);
        ValueTask<Host> RemoveHostByIdAsync(Guid hostId);
    }
}