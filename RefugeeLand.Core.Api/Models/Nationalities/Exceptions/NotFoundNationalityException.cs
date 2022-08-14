using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.Nationalities.Exceptions
{
    public class NotFoundNationalityException : Xeption
    {
        public NotFoundNationalityException(Guid nationalityId)
            : base(message: $"Couldn't find nationality with nationalityId: {nationalityId}.")
        { }
    }
}