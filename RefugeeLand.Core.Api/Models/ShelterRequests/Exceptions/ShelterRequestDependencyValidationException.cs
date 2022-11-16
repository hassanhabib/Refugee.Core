using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions
{
    public class ShelterRequestDependencyValidationException : Xeption
    {
        public ShelterRequestDependencyValidationException(Xeption innerException)
            : base(message: "ShelterRequest dependency validation occurred, please try again.", innerException)
        { }
    }
}