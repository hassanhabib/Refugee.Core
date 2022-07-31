using Xeptions;

namespace RefugeeLand.Core.Api.Models.hosts.Exceptions
{
    public class InvalidhostException : Xeption
    {
        public InvalidhostException()
            : base(message: "Invalid host. Please correct the errors and try again.")
        { }
    }
}