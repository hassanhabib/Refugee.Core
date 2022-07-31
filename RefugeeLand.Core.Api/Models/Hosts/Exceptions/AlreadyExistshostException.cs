using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.hosts.Exceptions
{
    public class AlreadyExistshostException : Xeption
    {
        public AlreadyExistshostException(Exception innerException)
            : base(message: "host with the same Id already exists.", innerException)
        { }
    }
}