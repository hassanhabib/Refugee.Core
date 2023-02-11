using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions
{
    public class NotFoundRefugeeGroupException : Xeption
    {
        public NotFoundRefugeeGroupException(Guid refugeeGroupId)
            : base(message: $"Couldn't find refugeeGroup with refugeeGroupId: {refugeeGroupId}.")
        { }
    }
}