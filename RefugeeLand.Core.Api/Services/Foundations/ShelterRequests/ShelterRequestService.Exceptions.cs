using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RefugeeLand.Core.Api.Models.ShelterRequests;
using RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions;
using Xeptions;

namespace RefugeeLand.Core.Api.Services.Foundations.ShelterRequests
{
    public partial class ShelterRequestService
    {
        private delegate ValueTask<ShelterRequest> ReturningShelterRequestFunction();
        private delegate IQueryable<ShelterRequest> ReturningShelterRequestsFunction();

        private async ValueTask<ShelterRequest> TryCatch(ReturningShelterRequestFunction returningShelterRequestFunction)
        {
            try
            {
                return await returningShelterRequestFunction();
            }
            catch (NullShelterRequestException nullShelterRequestException)
            {
                throw CreateAndLogValidationException(nullShelterRequestException);
            }
            catch (InvalidShelterRequestException invalidShelterRequestException)
            {
                throw CreateAndLogValidationException(invalidShelterRequestException);
            }
            catch (SqlException sqlException)
            {
                var failedShelterRequestStorageException =
                    new FailedShelterRequestStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedShelterRequestStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsShelterRequestException =
                    new AlreadyExistsShelterRequestException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsShelterRequestException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidShelterRequestReferenceException =
                    new InvalidShelterRequestReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidShelterRequestReferenceException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedShelterRequestStorageException =
                    new FailedShelterRequestStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedShelterRequestStorageException);
            }
            catch (Exception exception)
            {
                var failedShelterRequestServiceException =
                    new FailedShelterRequestServiceException(exception);

                throw CreateAndLogServiceException(failedShelterRequestServiceException);
            }
        }

        private IQueryable<ShelterRequest> TryCatch(ReturningShelterRequestsFunction returningShelterRequestsFunction)
        {
            try
            {
                return returningShelterRequestsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedShelterRequestStorageException =
                    new FailedShelterRequestStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedShelterRequestStorageException);
            }
            catch (Exception exception)
            {
                var failedShelterRequestServiceException =
                    new FailedShelterRequestServiceException(exception);

                throw CreateAndLogServiceException(failedShelterRequestServiceException);
            }
        }

        private ShelterRequestValidationException CreateAndLogValidationException(Xeption exception)
        {
            var shelterRequestValidationException =
                new ShelterRequestValidationException(exception);

            this.loggingBroker.LogError(shelterRequestValidationException);

            return shelterRequestValidationException;
        }

        private ShelterRequestDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var shelterRequestDependencyException = new ShelterRequestDependencyException(exception);
            this.loggingBroker.LogCritical(shelterRequestDependencyException);

            return shelterRequestDependencyException;
        }

        private ShelterRequestDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var shelterRequestDependencyValidationException =
                new ShelterRequestDependencyValidationException(exception);

            this.loggingBroker.LogError(shelterRequestDependencyValidationException);

            return shelterRequestDependencyValidationException;
        }

        private ShelterRequestDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var shelterRequestDependencyException = new ShelterRequestDependencyException(exception);
            this.loggingBroker.LogError(shelterRequestDependencyException);

            return shelterRequestDependencyException;
        }

        private ShelterRequestServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var shelterRequestServiceException = new ShelterRequestServiceException(exception);
            this.loggingBroker.LogError(shelterRequestServiceException);

            return shelterRequestServiceException;
        }
    }
}