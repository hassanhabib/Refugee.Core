using System.Threading.Tasks;
using RefugeeLand.Core.Api.Models.ShelterRequests;
using RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions;
using Xeptions;

namespace RefugeeLand.Core.Api.Services.Foundations.ShelterRequests
{
    public partial class ShelterRequestService
    {
        private delegate ValueTask<ShelterRequest> ReturningShelterRequestFunction();

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
        }

        private ShelterRequestValidationException CreateAndLogValidationException(Xeption exception)
        {
            var shelterRequestValidationException =
                new ShelterRequestValidationException(exception);

            this.loggingBroker.LogError(shelterRequestValidationException);

            return shelterRequestValidationException;
        }
    }
}