using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.hosts.Exceptions
{
    public class hostServiceException : Xeption
    {
        public hostServiceException(Exception innerException)
            : base(message: "host service error occurred, contact support.", innerException)
        { }
    }
}