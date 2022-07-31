using Xeptions;

namespace RefugeeLand.Core.Api.Models.Hosts.Exceptions
{
    public class HostValidationException : Xeption
    {
        public HostValidationException(Xeption innerException)
            : base(message: "Host validation errors occurred, please try again.",
                  innerException)
        { }
    }
}