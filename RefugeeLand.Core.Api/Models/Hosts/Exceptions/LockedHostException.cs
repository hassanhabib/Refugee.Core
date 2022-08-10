using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.Hosts.Exceptions
{
    public class LockedHostException : Xeption
    {
        public LockedHostException(Exception innerException)
            : base(message: "Locked host record exception, please try again later", innerException)
        {
        }
    }
}