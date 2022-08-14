// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RefugeeLand.Core.Api.Models.Nationalities;
using RefugeeLand.Core.Api.Models.Nationalities.Exceptions;
using Xeptions;

namespace RefugeeLand.Core.Api.Services.Foundations.Nationalities
{
    public partial class NationalityService
    {
        private delegate ValueTask<Nationality> ReturningNationalityFunction();
        private delegate IQueryable<Nationality> ReturningNationalitiesFunction();

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
            catch (NotFoundNationalityException notFoundNationalityException)
            {
                throw CreateAndLogValidationException(notFoundNationalityException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsNationalityException =
                    new AlreadyExistsNationalityException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsNationalityException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidNationalityReferenceException =
                    new InvalidNationalityReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidNationalityReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedNationalityException = new LockedNationalityException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedNationalityException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedNationalityStorageException =
                    new FailedNationalityStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedNationalityStorageException);
            }
            catch (Exception exception)
            {
                var failedNationalityServiceException =
                    new FailedNationalityServiceException(exception);

                throw CreateAndLogServiceException(failedNationalityServiceException);
            }
        }

        private IQueryable<Nationality> TryCatch(ReturningNationalitiesFunction returningNationalitiesFunction)
        {
            try
            {
                return returningNationalitiesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedNationalityStorageException =
                    new FailedNationalityStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedNationalityStorageException);
            }
            catch (Exception exception)
            {
                var failedNationalityServiceException =
                    new FailedNationalityServiceException(exception);

                throw CreateAndLogServiceException(failedNationalityServiceException);
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

        private NationalityDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var nationalityDependencyValidationException =
                new NationalityDependencyValidationException(exception);

            this.loggingBroker.LogError(nationalityDependencyValidationException);

            return nationalityDependencyValidationException;
        }

        private NationalityDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var nationalityDependencyException = new NationalityDependencyException(exception);
            this.loggingBroker.LogError(nationalityDependencyException);

            return nationalityDependencyException;
        }

        private NationalityServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var nationalityServiceException = new NationalityServiceException(exception);
            this.loggingBroker.LogError(nationalityServiceException);

            return nationalityServiceException;
        }
    }
}