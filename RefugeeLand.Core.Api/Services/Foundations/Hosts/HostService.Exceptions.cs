using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RefugeeLand.Core.Api.Models.Hosts;
using RefugeeLand.Core.Api.Models.Hosts.Exceptions;
using Xeptions;

namespace RefugeeLand.Core.Api.Services.Foundations.Hosts
{
    public partial class HostService
    {
        private delegate ValueTask<Host> ReturningHostFunction();
        private delegate IQueryable<Host> ReturningHostsFunction();

        private async ValueTask<Host> TryCatch(ReturningHostFunction returningHostFunction)
        {
            try
            {
                return await returningHostFunction();
            }
            catch (NullHostException nullHostException)
            {
                throw CreateAndLogValidationException(nullHostException);
            }
            catch (InvalidHostException invalidHostException)
            {
                throw CreateAndLogValidationException(invalidHostException);
            }
            catch (SqlException sqlException)
            {
                var failedHostStorageException =
                    new FailedHostStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedHostStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsHostException =
                    new AlreadyExistsHostException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsHostException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidHostReferenceException =
                    new InvalidHostReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidHostReferenceException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedHostStorageException =
                    new FailedHostStorageException(databaseUpdateException);

                throw CreateAndLogDependecyException(failedHostStorageException);
            }
            catch (Exception exception)
            {
                var failedHostServiceException =
                    new FailedHostServiceException(exception);

                throw CreateAndLogServiceException(failedHostServiceException);
            }
        }

        private IQueryable<Host> TryCatch(ReturningHostsFunction returningHostsFunction)
        {
            try
            {
                return returningHostsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedHostStorageException =
                    new FailedHostStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedHostStorageException);
            }
        }

        private HostValidationException CreateAndLogValidationException(Xeption exception)
        {
            var hostValidationException =
                new HostValidationException(exception);

            this.loggingBroker.LogError(hostValidationException);

            return hostValidationException;
        }

        private HostDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var hostDependencyException = new HostDependencyException(exception);
            this.loggingBroker.LogCritical(hostDependencyException);

            return hostDependencyException;
        }

        private HostDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var hostDependencyValidationException =
                new HostDependencyValidationException(exception);

            this.loggingBroker.LogError(hostDependencyValidationException);

            return hostDependencyValidationException;
        }

        private HostDependencyException CreateAndLogDependecyException(
            Xeption exception)
        {
            var hostDependencyException = new HostDependencyException(exception);
            this.loggingBroker.LogError(hostDependencyException);

            return hostDependencyException;
        }

        private HostServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var hostServiceException = new HostServiceException(exception);
            this.loggingBroker.LogError(hostServiceException);

            return hostServiceException;
        }
    }
}