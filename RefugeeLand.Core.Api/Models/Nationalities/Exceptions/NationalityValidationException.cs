using Xeptions;

namespace RefugeeLand.Core.Api.Models.Nationalities.Exceptions
{
    public class NationalityValidationException : Xeption
    {
        public NationalityValidationException(Xeption innerException)
            : base(message: "Nationality validation errors occurred, please try again.",
                  innerException)
        { }
    }
}