using Xeptions;

namespace RefugeeLand.Core.Api.Models.Hosts.Exceptions
{
    public class InvalidHostException : Xeption
    {
        public InvalidHostException()
            : base(message: "Invalid host. Please correct the errors and try again.")
        { }
    }
}