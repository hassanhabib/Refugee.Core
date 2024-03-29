// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Brokers.DateTimes;
using RefugeeLand.Core.Api.Brokers.Loggings;
using RefugeeLand.Core.Api.Brokers.Storages;
using RefugeeLand.Core.Api.Models.Hosts;

namespace RefugeeLand.Core.Api.Services.Foundations.Hosts
{
    public partial class HostService : IHostService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public HostService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Host> AddHostAsync(Host host) =>
            TryCatch(async () =>
            {
                ValidateHostOnAdd(host);

                return await this.storageBroker.InsertHostAsync(host);
            });

        public IQueryable<Host> RetrieveAllHosts() =>
            TryCatch(() => this.storageBroker.SelectAllHosts());

        public ValueTask<Host> RetrieveHostByIdAsync(Guid hostId) =>
            TryCatch(async () =>
            {
                ValidateHostId(hostId);

                Host maybeHost = await this.storageBroker
                    .SelectHostByIdAsync(hostId);

                ValidateStorageHost(maybeHost, hostId);

                return maybeHost;
            });

        public ValueTask<Host> ModifyHostAsync(Host host) =>
            TryCatch(async () =>
            {
                ValidateHostOnModify(host);

                Host maybeHost =
                    await this.storageBroker.SelectHostByIdAsync(host.Id);

                ValidateStorageHost(maybeHost, host.Id);
                ValidateAgainstStorageHostOnModify(inputHost: host, storageHost: maybeHost);

                return await this.storageBroker.UpdateHostAsync(host);
            });

        public ValueTask<Host> RemoveHostByIdAsync(Guid hostId) =>
            TryCatch(async () =>
            {
                ValidateHostId(hostId);

                Host maybeHost = await this.storageBroker
                    .SelectHostByIdAsync(hostId);

                ValidateStorageHost(maybeHost, hostId);

                return await this.storageBroker.DeleteHostAsync(maybeHost);
            });
    }
}