using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.Nationalities.Exceptions
{
    public class FailedNationalityServiceException : Xeption
    {
        public FailedNationalityServiceException(Exception innerException)
            : base(message: "Failed nationality service occurred, please contact support", innerException)
        { }
    }
}