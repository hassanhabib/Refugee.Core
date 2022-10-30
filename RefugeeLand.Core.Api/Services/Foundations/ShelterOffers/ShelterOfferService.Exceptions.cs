using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using RefugeeLand.Core.Api.Models.ShelterOffers;
using RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions;
using Xeptions;

namespace RefugeeLand.Core.Api.Services.Foundations.ShelterOffers
{
    public partial class ShelterOfferService
    {
        private delegate ValueTask<ShelterOffer> ReturningShelterOfferFunction();

        private async ValueTask<ShelterOffer> TryCatch(ReturningShelterOfferFunction returningShelterOfferFunction)
        {
            try
            {
                return await returningShelterOfferFunction();
            }
            catch (NullShelterOfferException nullShelterOfferException)
            {
                throw CreateAndLogValidationException(nullShelterOfferException);
            }
            catch (InvalidShelterOfferException invalidShelterOfferException)
            {
                throw CreateAndLogValidationException(invalidShelterOfferException);
            }
            catch (SqlException sqlException)
            {
                var failedShelterOfferStorageException =
                    new FailedShelterOfferStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedShelterOfferStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsShelterOfferException =
                    new AlreadyExistsShelterOfferException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsShelterOfferException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidShelterOfferReferenceException =
                    new InvalidShelterOfferReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidShelterOfferReferenceException);
            }
        }

        private ShelterOfferValidationException CreateAndLogValidationException(Xeption exception)
        {
            var shelterOfferValidationException =
                new ShelterOfferValidationException(exception);

            this.loggingBroker.LogError(shelterOfferValidationException);

            return shelterOfferValidationException;
        }

        private ShelterOfferDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var shelterOfferDependencyException = new ShelterOfferDependencyException(exception);
            this.loggingBroker.LogCritical(shelterOfferDependencyException);

            return shelterOfferDependencyException;
        }

        private ShelterOfferDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var shelterOfferDependencyValidationException =
                new ShelterOfferDependencyValidationException(exception);

            this.loggingBroker.LogError(shelterOfferDependencyValidationException);

            return shelterOfferDependencyValidationException;
        }
    }
}