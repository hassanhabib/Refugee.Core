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

        public async ValueTask<ShelterRequest> RetrieveShelterRequestByIdAsync(Guid shelterRequestId) =>
            await this.storageBroker.SelectShelterRequestByIdAsync(shelterRequestId);
    }
}