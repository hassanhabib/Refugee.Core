using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions
{
    public class FailedShelterRequestStorageException : Xeption
    {
        public FailedShelterRequestStorageException(Exception innerException)
            : base(message: "Failed shelterRequest storage error occurred, contact support.", innerException)
        { }
    }
}