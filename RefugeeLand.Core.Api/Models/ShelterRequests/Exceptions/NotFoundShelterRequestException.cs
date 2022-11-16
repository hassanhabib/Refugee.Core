using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions
{
    public class NotFoundShelterRequestException : Xeption
    {
        public NotFoundShelterRequestException(Guid shelterRequestId)
            : base(message: $"Couldn't find shelterRequest with shelterRequestId: {shelterRequestId}.")
        { }
    }
}