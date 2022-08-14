using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.Nationalities;
using RefugeeLand.Core.Api.Models.Nationalities.Exceptions;
using Xeptions;

namespace RefugeeLand.Core.Api.Services.Foundations.Nationalities
{
    public partial class NationalityService
    {
        private delegate ValueTask<Nationality> ReturningNationalityFunction();

        private async ValueTask<Nationality> TryCatch(ReturningNationalityFunction returningNationalityFunction)
        {
            try
            {
                return await returningNationalityFunction();
            }
            catch (NullNationalityException nullNationalityException)
            {
                throw CreateAndLogValidationException(nullNationalityException);
            }
        }

        private NationalityValidationException CreateAndLogValidationException(Xeption exception)
        {
            var nationalityValidationException =
                new NationalityValidationException(exception);

            this.loggingBroker.LogError(nationalityValidationException);

            return nationalityValidationException;
        }
    }
}