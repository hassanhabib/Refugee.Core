using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.Hosts.Exceptions
{
    public class NotFoundHostException : Xeption
    {
        public NotFoundHostException(Guid hostId)
            : base(message: $"Couldn't find host with hostId: {hostId}.")
        { }
    }
}