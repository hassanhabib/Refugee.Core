using Xeptions;

namespace RefugeeLand.Core.Api.Models.Nationalities.Exceptions
{
    public class NullNationalityException : Xeption
    {
        public NullNationalityException()
            : base(message: "Nationality is null.")
        { }
    }
}