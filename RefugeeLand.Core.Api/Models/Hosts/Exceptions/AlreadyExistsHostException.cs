using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.Hosts.Exceptions
{
    public class AlreadyExistsHostException : Xeption
    {
        public AlreadyExistsHostException(Exception innerException)
            : base(message: "Host with the same Id already exists.", innerException)
        { }
    }
}