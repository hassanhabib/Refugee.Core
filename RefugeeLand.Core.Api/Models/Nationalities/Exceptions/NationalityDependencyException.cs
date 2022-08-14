using Xeptions;

namespace RefugeeLand.Core.Api.Models.Nationalities.Exceptions
{
    public class NationalityDependencyException : Xeption
    {
        public NationalityDependencyException(Xeption innerException) :
            base(message: "Nationality dependency error occurred, contact support.", innerException)
        { }
    }
}