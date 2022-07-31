using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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
    }
}