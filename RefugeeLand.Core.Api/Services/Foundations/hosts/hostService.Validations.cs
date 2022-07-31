using RefugeeLand.Core.Api.Models.hosts;
using RefugeeLand.Core.Api.Models.hosts.Exceptions;

namespace RefugeeLand.Core.Api.Services.Foundations.hosts
{
    public partial class hostService
    {
        private void ValidatehostOnAdd(host host)
        {
            ValidatehostIsNotNull(host);
        }

        private static void ValidatehostIsNotNull(host host)
        {
            if (host is null)
            {
                throw new NullhostException();
            }
        }
    }
}