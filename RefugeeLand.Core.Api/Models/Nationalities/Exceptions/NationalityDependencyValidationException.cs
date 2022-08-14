using Xeptions;

namespace RefugeeLand.Core.Api.Models.Nationalities.Exceptions
{
    public class NationalityDependencyValidationException : Xeption
    {
        public NationalityDependencyValidationException(Xeption innerException)
            : base(message: "Nationality dependency validation occurred, please try again.", innerException)
        { }
    }
}