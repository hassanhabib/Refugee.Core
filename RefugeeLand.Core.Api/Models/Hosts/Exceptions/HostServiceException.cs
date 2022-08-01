using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.Hosts.Exceptions
{
    public class HostServiceException : Xeption
    {
        public HostServiceException(Exception innerException)
            : base(message: "Host service error occurred, contact support.", innerException)
        { }
    }
}