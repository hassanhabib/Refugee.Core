using RefugeeLand.Core.Api.Models.Nationalities;
using RefugeeLand.Core.Api.Models.Nationalities.Exceptions;

namespace RefugeeLand.Core.Api.Services.Foundations.Nationalities
{
    public partial class NationalityService
    {
        private void ValidateNationalityOnAdd(Nationality nationality)
        {
            ValidateNationalityIsNotNull(nationality);
        }

        private static void ValidateNationalityIsNotNull(Nationality nationality)
        {
            if (nationality is null)
            {
                throw new NullNationalityException();
            }
        }
    }
}