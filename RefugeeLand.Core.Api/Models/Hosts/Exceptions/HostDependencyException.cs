using Xeptions;

namespace RefugeeLand.Core.Api.Models.Hosts.Exceptions
{
    public class HostDependencyException : Xeption
    {
        public HostDependencyException(Xeption innerException) :
            base(message: "Host dependency error occurred, contact support.", innerException)
        { }
    }
}