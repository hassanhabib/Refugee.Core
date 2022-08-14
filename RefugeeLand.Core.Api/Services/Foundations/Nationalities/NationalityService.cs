using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Brokers.DateTimes;
using RefugeeLand.Core.Api.Brokers.Loggings;
using RefugeeLand.Core.Api.Brokers.Storages;
using RefugeeLand.Core.Api.Models.Nationalities;

namespace RefugeeLand.Core.Api.Services.Foundations.Nationalities
{
    public partial class NationalityService : INationalityService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public NationalityService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Nationality> AddNationalityAsync(Nationality nationality) =>
            TryCatch(async () =>
            {
                ValidateNationalityOnAdd(nationality);

                return await this.storageBroker.InsertNationalityAsync(nationality);
            });

        public IQueryable<Nationality> RetrieveAllNationalities() =>
            TryCatch(() => this.storageBroker.SelectAllNationalities());

        public ValueTask<Nationality> RetrieveNationalityByIdAsync(Guid nationalityId) =>
            TryCatch(async () =>
            {
                ValidateNationalityId(nationalityId);

                Nationality maybeNationality = await this.storageBroker
                    .SelectNationalityByIdAsync(nationalityId);

                ValidateStorageNationality(maybeNationality, nationalityId);

                return maybeNationality;
            });

        public ValueTask<Nationality> ModifyNationalityAsync(Nationality nationality) =>
            TryCatch(async () =>
            {
                ValidateNationalityOnModify(nationality);

                Nationality maybeNationality =
                    await this.storageBroker.SelectNationalityByIdAsync(nationality.Id);

                ValidateStorageNationality(maybeNationality, nationality.Id);
                ValidateAgainstStorageNationalityOnModify(inputNationality: nationality, storageNationality: maybeNationality);

                return await this.storageBroker.UpdateNationalityAsync(nationality);
            });

        public async ValueTask<Nationality> RemoveNationalityByIdAsync(Guid nationalityId)
        {
            Nationality maybeNationality = await this.storageBroker
                    .SelectNationalityByIdAsync(nationalityId);

            return await this.storageBroker.DeleteNationalityAsync(maybeNationality);
        }
    }
}