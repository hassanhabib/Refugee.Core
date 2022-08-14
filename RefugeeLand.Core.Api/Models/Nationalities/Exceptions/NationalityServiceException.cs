using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.Nationalities.Exceptions
{
    public class NationalityServiceException : Xeption
    {
        public NationalityServiceException(Exception innerException)
            : base(message: "Nationality service error occurred, contact support.", innerException)
        { }
    }
}