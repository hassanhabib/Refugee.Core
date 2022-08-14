using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.Nationalities.Exceptions
{
    public class AlreadyExistsNationalityException : Xeption
    {
        public AlreadyExistsNationalityException(Exception innerException)
            : base(message: "Nationality with the same Id already exists.", innerException)
        { }
    }
}