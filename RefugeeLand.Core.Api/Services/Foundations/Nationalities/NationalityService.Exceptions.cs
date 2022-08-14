using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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
            catch (InvalidNationalityException invalidNationalityException)
            {
                throw CreateAndLogValidationException(invalidNationalityException);
            }
            catch (SqlException sqlException)
            {
                var failedNationalityStorageException =
                    new FailedNationalityStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedNationalityStorageException);
            }
        }

        private NationalityValidationException CreateAndLogValidationException(Xeption exception)
        {
            var nationalityValidationException =
                new NationalityValidationException(exception);

            this.loggingBroker.LogError(nationalityValidationException);

            return nationalityValidationException;
        }

        private NationalityDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var nationalityDependencyException = new NationalityDependencyException(exception);
            this.loggingBroker.LogCritical(nationalityDependencyException);

            return nationalityDependencyException;
        }
    }
}