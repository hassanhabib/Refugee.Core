using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions
{
    public class ShelterRequestDependencyException : Xeption
    {
        public ShelterRequestDependencyException(Xeption innerException) :
            base(message: "ShelterRequest dependency error occurred, contact support.", innerException)
        { }
    }
}