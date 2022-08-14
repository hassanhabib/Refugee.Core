using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.Nationalities.Exceptions
{
    public class LockedNationalityException : Xeption
    {
        public LockedNationalityException(Exception innerException)
            : base(message: "Locked nationality record exception, please try again later", innerException)
        {
        }
    }
}