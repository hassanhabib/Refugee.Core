// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System.Threading.Tasks;
using RefugeeLand.Core.Api.Brokers.DateTimes;
using RefugeeLand.Core.Api.Brokers.Storages;
using RefugeeLand.Core.Api.Models.Refugees;

namespace RefugeeLand.Core.Api.Services.Foundations.Refugees
{
    public class RefugeeService : IRefugeeService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public RefugeeService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public async ValueTask<Refugee> AddRefugeeAsync(Refugee refugee) =>
            await this.storageBroker.InsertRefugeeAsync(refugee);
    }
}
