using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.hosts.Exceptions
{
    public class NotFoundhostException : Xeption
    {
        public NotFoundhostException(Guid hostId)
            : base(message: $"Couldn't find host with hostId: {hostId}.")
        { }
    }
}