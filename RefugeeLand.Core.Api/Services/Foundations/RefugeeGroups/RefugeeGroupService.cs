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
using RefugeeLand.Core.Api.Models.RefugeeGroups;

namespace RefugeeLand.Core.Api.Services.Foundations.RefugeeGroups
{
    public partial class RefugeeGroupService : IRefugeeGroupService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public RefugeeGroupService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<RefugeeGroup> AddRefugeeGroupAsync(RefugeeGroup refugeeGroup) =>
            TryCatch(async () =>
            {
                ValidateRefugeeGroup(refugeeGroup);

                return await this.storageBroker.InsertRefugeeGroupAsync(refugeeGroup);
            });

        public IQueryable<RefugeeGroup> RetrieveAllRefugeeGroups() =>
        TryCatch(() => this.storageBroker.SelectAllRefugeeGroups());

        public ValueTask<RefugeeGroup> RetrieveRefugeeGroupByIdAsync(Guid refugeeGroupId) =>
        TryCatch(async () =>
        {
            ValidateRefugeeGroupId(refugeeGroupId);

            RefugeeGroup maybeRefugeeGroup = await this.storageBroker
                .SelectRefugeeGroupByIdAsync(refugeeGroupId);

            ValidateStorageRefugeeGroup(maybeRefugeeGroup, refugeeGroupId);

            return maybeRefugeeGroup;
        });

        public ValueTask<RefugeeGroup> ModifyRefugeeGroupAsync(RefugeeGroup refugeeGroup) =>
        TryCatch(async () =>
        {
            ValidateRefugeeGroupOnModify(refugeeGroup);

            RefugeeGroup maybeRefugeeGroup = await this.storageBroker
                .UpdateRefugeeGroupAsync(refugeeGroup);

            ValidateStorageRefugeeGroup(maybeRefugeeGroup, refugeeGroup.Id);

            return maybeRefugeeGroup;
        });
    }
}