using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions
{
    public class ShelterRequestValidationException : Xeption
    {
        public ShelterRequestValidationException(Xeption innerException)
            : base(message: "ShelterRequest validation errors occurred, please try again.",
                  innerException)
        { }
    }
}