using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Brokers.DateTimes;
using RefugeeLand.Core.Api.Brokers.Loggings;
using RefugeeLand.Core.Api.Brokers.Storages;
using RefugeeLand.Core.Api.Models.hosts;

namespace RefugeeLand.Core.Api.Services.Foundations.hosts
{
    public partial class hostService : IhostService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public hostService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<host> AddhostAsync(host host) =>
            TryCatch(async () =>
            {
                ValidatehostOnAdd(host);

                return await this.storageBroker.InserthostAsync(host);
            });

        public IQueryable<host> RetrieveAllhosts() =>
            TryCatch(() => this.storageBroker.SelectAllhosts());

        public ValueTask<host> RetrievehostByIdAsync(Guid hostId) =>
            TryCatch(async () =>
            {
                ValidatehostId(hostId);

                host maybehost = await this.storageBroker
                    .SelecthostByIdAsync(hostId);

                ValidateStoragehost(maybehost, hostId);

                return maybehost;
            });

        public ValueTask<host> ModifyhostAsync(host host) =>
            TryCatch(async () =>
            {
                ValidatehostOnModify(host);

                host maybehost =
                    await this.storageBroker.SelecthostByIdAsync(host.Id);

                ValidateStoragehost(maybehost, host.Id);
                ValidateAgainstStoragehostOnModify(inputhost: host, storagehost: maybehost);

                return await this.storageBroker.UpdatehostAsync(host);
            });

        public async ValueTask<host> RemovehostByIdAsync(Guid hostId)
        {
            host maybehost = await this.storageBroker
                    .SelecthostByIdAsync(hostId);

            return await this.storageBroker.DeletehostAsync(maybehost);
        }
    }
}