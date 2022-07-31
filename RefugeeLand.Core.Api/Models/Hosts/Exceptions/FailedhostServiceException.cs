using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.hosts.Exceptions
{
    public class FailedhostServiceException : Xeption
    {
        public FailedhostServiceException(Exception innerException)
            : base(message: "Failed host service occurred, please contact support", innerException)
        { }
    }
}