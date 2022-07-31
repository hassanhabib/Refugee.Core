using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.hosts;

namespace RefugeeLand.Core.Api.Services.Foundations.hosts
{
    public interface IhostService
    {
        ValueTask<host> AddhostAsync(host host);
        IQueryable<host> RetrieveAllhosts();
        ValueTask<host> RetrievehostByIdAsync(Guid hostId);
        ValueTask<host> ModifyhostAsync(host host);
    }
}