using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions
{
    public class AlreadyExistsShelterRequestException : Xeption
    {
        public AlreadyExistsShelterRequestException(Exception innerException)
            : base(message: "ShelterRequest with the same Id already exists.", innerException)
        { }
    }
}