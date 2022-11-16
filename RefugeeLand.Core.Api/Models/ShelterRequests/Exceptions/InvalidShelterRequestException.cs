using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions
{
    public class InvalidShelterRequestException : Xeption
    {
        public InvalidShelterRequestException()
            : base(message: "Invalid shelterRequest. Please correct the errors and try again.")
        { }
    }
}