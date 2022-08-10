using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using RefugeeLand.Core.Api.Models.Hosts;
using RefugeeLand.Core.Api.Models.Hosts.Exceptions;
using Xeptions;

namespace RefugeeLand.Core.Api.Services.Foundations.Hosts
{
    public partial class HostService
    {
        private delegate ValueTask<Host> ReturningHostFunction();

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
    }
}