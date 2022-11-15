using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions
{
    public class LockedShelterRequestException : Xeption
    {
        public LockedShelterRequestException(Exception innerException)
            : base(message: "Locked shelterRequest record exception, please try again later", innerException)
        {
        }
    }
}