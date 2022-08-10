using Xeptions;

namespace RefugeeLand.Core.Api.Models.Hosts.Exceptions
{
    public class NullHostException : Xeption
    {
        public NullHostException()
            : base(message: "Host is null.")
        { }
    }
}