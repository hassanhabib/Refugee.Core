using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.hosts.Exceptions
{
    public class FailedhostStorageException : Xeption
    {
        public FailedhostStorageException(Exception innerException)
            : base(message: "Failed host storage error occurred, contact support.", innerException)
        { }
    }
}