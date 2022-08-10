using Xeptions;

namespace RefugeeLand.Core.Api.Models.Hosts.Exceptions
{
    public class HostDependencyValidationException : Xeption
    {
        public HostDependencyValidationException(Xeption innerException)
            : base(message: "Host dependency validation occurred, please try again.", innerException)
        { }
    }
}