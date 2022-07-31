using Xeptions;

namespace RefugeeLand.Core.Api.Models.hosts.Exceptions
{
    public class hostValidationException : Xeption
    {
        public hostValidationException(Xeption innerException)
            : base(message: "host validation errors occurred, please try again.",
                  innerException)
        { }
    }
}