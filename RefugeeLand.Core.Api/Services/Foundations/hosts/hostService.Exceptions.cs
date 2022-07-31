using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RefugeeLand.Core.Api.Models.hosts;
using RefugeeLand.Core.Api.Models.hosts.Exceptions;
using Xeptions;

namespace RefugeeLand.Core.Api.Services.Foundations.hosts
{
    public partial class hostService
    {
        private delegate ValueTask<host> ReturninghostFunction();

        private async ValueTask<host> TryCatch(ReturninghostFunction returninghostFunction)
        {
            try
            {
                return await returninghostFunction();
            }
            catch (NullhostException nullhostException)
            {
                throw CreateAndLogValidationException(nullhostException);
            }
            catch (InvalidhostException invalidhostException)
            {
                throw CreateAndLogValidationException(invalidhostException);
            }
            catch (SqlException sqlException)
            {
                var failedhostStorageException =
                    new FailedhostStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedhostStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistshostException =
                    new AlreadyExistshostException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistshostException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidhostReferenceException =
                    new InvalidhostReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidhostReferenceException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedhostStorageException =
                    new FailedhostStorageException(databaseUpdateException);

                throw CreateAndLogDependecyException(failedhostStorageException);
            }
        }

        private hostValidationException CreateAndLogValidationException(Xeption exception)
        {
            var hostValidationException =
                new hostValidationException(exception);

            this.loggingBroker.LogError(hostValidationException);

            return hostValidationException;
        }

        private hostDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var hostDependencyException = new hostDependencyException(exception);
            this.loggingBroker.LogCritical(hostDependencyException);

            return hostDependencyException;
        }

        private hostDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var hostDependencyValidationException =
                new hostDependencyValidationException(exception);

            this.loggingBroker.LogError(hostDependencyValidationException);

            return hostDependencyValidationException;
        }

        private hostDependencyException CreateAndLogDependecyException(
            Xeption exception)
        {
            var hostDependencyException = new hostDependencyException(exception);
            this.loggingBroker.LogError(hostDependencyException);

            return hostDependencyException;
        }
    }
}