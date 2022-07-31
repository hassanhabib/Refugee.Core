using Xeptions;

namespace RefugeeLand.Core.Api.Models.hosts.Exceptions
{
    public class hostDependencyException : Xeption
    {
        public hostDependencyException(Xeption innerException) :
            base(message: "host dependency error occurred, contact support.", innerException)
        { }
    }
}