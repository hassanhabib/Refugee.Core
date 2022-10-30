using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Brokers.DateTimes;
using RefugeeLand.Core.Api.Brokers.Loggings;
using RefugeeLand.Core.Api.Brokers.Storages;
using RefugeeLand.Core.Api.Models.ShelterOffers;

namespace RefugeeLand.Core.Api.Services.Foundations.ShelterOffers
{
    public partial class ShelterOfferService : IShelterOfferService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ShelterOfferService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ShelterOffer> AddShelterOfferAsync(ShelterOffer shelterOffer) =>
            TryCatch(async () =>
            {
                ValidateShelterOfferOnAdd(shelterOffer);

                return await this.storageBroker.InsertShelterOfferAsync(shelterOffer);
            });

        public IQueryable<ShelterOffer> RetrieveAllShelterOffers() =>
            TryCatch(() => this.storageBroker.SelectAllShelterOffers());

        public ValueTask<ShelterOffer> RetrieveShelterOfferByIdAsync(Guid shelterOfferId) =>
            TryCatch(async () =>
            {
                ValidateShelterOfferId(shelterOfferId);

                ShelterOffer maybeShelterOffer = await this.storageBroker
                    .SelectShelterOfferByIdAsync(shelterOfferId);

                ValidateStorageShelterOffer(maybeShelterOffer, shelterOfferId);

                return maybeShelterOffer;
            });

        public ValueTask<ShelterOffer> ModifyShelterOfferAsync(ShelterOffer shelterOffer) =>
            TryCatch(async () =>
            {
                ValidateShelterOfferOnModify(shelterOffer);

                return await this.storageBroker.UpdateShelterOfferAsync(shelterOffer);
            });
    }
}