using Xeptions;

namespace RefugeeLand.Core.Api.Models.hosts.Exceptions
{
    public class hostDependencyValidationException : Xeption
    {
        public hostDependencyValidationException(Xeption innerException)
            : base(message: "host dependency validation occurred, please try again.", innerException)
        { }
    }
}