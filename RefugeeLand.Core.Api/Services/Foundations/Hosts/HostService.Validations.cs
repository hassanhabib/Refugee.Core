using RefugeeLand.Core.Api.Models.Hosts;
using RefugeeLand.Core.Api.Models.Hosts.Exceptions;

namespace RefugeeLand.Core.Api.Services.Foundations.Hosts
{
    public partial class HostService
    {
        private void ValidateHostOnAdd(Host host)
        {
            ValidateHostIsNotNull(host);
        }

        private static void ValidateHostIsNotNull(Host host)
        {
            if (host is null)
            {
                throw new NullHostException();
            }
        }
    }
}