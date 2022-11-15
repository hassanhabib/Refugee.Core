using RefugeeLand.Core.Api.Models.ShelterRequests;
using RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions;

namespace RefugeeLand.Core.Api.Services.Foundations.ShelterRequests
{
    public partial class ShelterRequestService
    {
        private void ValidateShelterRequestOnAdd(ShelterRequest shelterRequest)
        {
            ValidateShelterRequestIsNotNull(shelterRequest);
        }

        private static void ValidateShelterRequestIsNotNull(ShelterRequest shelterRequest)
        {
            if (shelterRequest is null)
            {
                throw new NullShelterRequestException();
            }
        }
    }
}