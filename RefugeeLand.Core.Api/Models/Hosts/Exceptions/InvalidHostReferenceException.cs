using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.Hosts.Exceptions
{
    public class InvalidHostReferenceException : Xeption
    {
        public InvalidHostReferenceException(Exception innerException)
            : base(message: "Invalid host reference error occurred.", innerException) { }
    }
}