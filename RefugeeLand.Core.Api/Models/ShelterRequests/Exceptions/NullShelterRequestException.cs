using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions
{
    public class NullShelterRequestException : Xeption
    {
        public NullShelterRequestException()
            : base(message: "ShelterRequest is null.")
        { }
    }
}