using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions
{
    public class ShelterRequestServiceException : Xeption
    {
        public ShelterRequestServiceException(Exception innerException)
            : base(message: "ShelterRequest service error occurred, contact support.", innerException)
        { }
    }
}