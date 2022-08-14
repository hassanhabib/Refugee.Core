using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.Nationalities.Exceptions
{
    public class FailedNationalityStorageException : Xeption
    {
        public FailedNationalityStorageException(Exception innerException)
            : base(message: "Failed nationality storage error occurred, contact support.", innerException)
        { }
    }
}