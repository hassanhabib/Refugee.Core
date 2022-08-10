using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.Hosts.Exceptions
{
    public class FailedHostStorageException : Xeption
    {
        public FailedHostStorageException(Exception innerException)
            : base(message: "Failed host storage error occurred, contact support.", innerException)
        { }
    }
}