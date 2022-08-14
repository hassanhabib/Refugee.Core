using Xeptions;

namespace RefugeeLand.Core.Api.Models.Nationalities.Exceptions
{
    public class InvalidNationalityException : Xeption
    {
        public InvalidNationalityException()
            : base(message: "Invalid nationality. Please correct the errors and try again.")
        { }
    }
}