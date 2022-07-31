using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.hosts.Exceptions
{
    public class InvalidhostReferenceException : Xeption
    {
        public InvalidhostReferenceException(Exception innerException)
            : base(message: "Invalid host reference error occurred.", innerException) { }
    }
}