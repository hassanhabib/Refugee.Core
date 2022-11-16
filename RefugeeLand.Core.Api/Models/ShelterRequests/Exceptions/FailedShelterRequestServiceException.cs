using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions
{
    public class FailedShelterRequestServiceException : Xeption
    {
        public FailedShelterRequestServiceException(Exception innerException)
            : base(message: "Failed shelterRequest service occurred, please contact support", innerException)
        { }
    }
}