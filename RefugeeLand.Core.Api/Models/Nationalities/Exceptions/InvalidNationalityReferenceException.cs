using System;
using Xeptions;

namespace RefugeeLand.Core.Api.Models.Nationalities.Exceptions
{
    public class InvalidNationalityReferenceException : Xeption
    {
        public InvalidNationalityReferenceException(Exception innerException)
            : base(message: "Invalid nationality reference error occurred.", innerException) { }
    }
}