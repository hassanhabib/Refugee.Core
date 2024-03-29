using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Brokers.DateTimes;
using RefugeeLand.Core.Api.Brokers.Loggings;
using RefugeeLand.Core.Api.Brokers.Storages;
using RefugeeLand.Core.Api.Models.ShelterRequests;

namespace RefugeeLand.Core.Api.Services.Foundations.ShelterRequests
{
    public partial class ShelterRequestService : IShelterRequestService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ShelterRequestService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ShelterRequest> AddShelterRequestAsync(ShelterRequest shelterRequest) =>
            TryCatch(async () =>
            {
                ValidateShelterRequestOnAdd(shelterRequest);

                return await this.storageBroker.InsertShelterRequestAsync(shelterRequest);
            });

        public IQueryable<ShelterRequest> RetrieveAllShelterRequests() =>
            TryCatch(() => this.storageBroker.SelectAllShelterRequests());

        public ValueTask<ShelterRequest> RetrieveShelterRequestByIdAsync(Guid shelterRequestId) =>
            TryCatch(async () =>
            {
                ValidateShelterRequestId(shelterRequestId);

                ShelterRequest maybeShelterRequest = await this.storageBroker
                    .SelectShelterRequestByIdAsync(shelterRequestId);

                ValidateStorageShelterRequest(maybeShelterRequest, shelterRequestId);

                return maybeShelterRequest;
            });

        public ValueTask<ShelterRequest> ModifyShelterRequestAsync(ShelterRequest shelterRequest) =>
            TryCatch(async () =>
            {
                ValidateShelterRequestOnModify(shelterRequest);

                ShelterRequest maybeShelterRequest =
                    await this.storageBroker.SelectShelterRequestByIdAsync(shelterRequest.Id);

                ValidateStorageShelterRequest(maybeShelterRequest, shelterRequest.Id);
                ValidateAgainstStorageShelterRequestOnModify(inputShelterRequest: shelterRequest, storageShelterRequest: maybeShelterRequest);

                return await this.storageBroker.UpdateShelterRequestAsync(shelterRequest);
            });

        public ValueTask<ShelterRequest> RemoveShelterRequestByIdAsync(Guid shelterRequestId) =>
            TryCatch(async () =>
            {
                ValidateShelterRequestId(shelterRequestId);

                ShelterRequest maybeShelterRequest = await this.storageBroker
                    .SelectShelterRequestByIdAsync(shelterRequestId);

                ValidateStorageShelterRequest(maybeShelterRequest, shelterRequestId);

                return await this.storageBroker.DeleteShelterRequestAsync(maybeShelterRequest);
            });
    }
}