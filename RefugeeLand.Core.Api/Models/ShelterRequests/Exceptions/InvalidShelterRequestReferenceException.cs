using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions
{
    public class InvalidShelterRequestReferenceException : Xeption
    {
        public InvalidShelterRequestReferenceException(Exception innerException)
            : base(message: "Invalid shelterRequest reference error occurred.", innerException) { }
    }
}