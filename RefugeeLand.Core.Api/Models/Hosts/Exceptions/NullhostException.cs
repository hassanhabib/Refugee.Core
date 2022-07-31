using Xeptions;

namespace RefugeeLand.Core.Api.Models.hosts.Exceptions
{
    public class NullhostException : Xeption
    {
        public NullhostException()
            : base(message: "host is null.")
        { }
    }
}