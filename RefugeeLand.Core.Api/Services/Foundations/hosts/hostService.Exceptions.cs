using System.Threading.Tasks;
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
        }

        private hostValidationException CreateAndLogValidationException(Xeption exception)
        {
            var hostValidationException =
                new hostValidationException(exception);

            this.loggingBroker.LogError(hostValidationException);

            return hostValidationException;
        }
    }
}