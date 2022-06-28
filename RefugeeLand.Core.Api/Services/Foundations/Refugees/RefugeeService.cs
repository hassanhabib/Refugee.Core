// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using RefugeeLand.Core.Api.Brokers.DateTimes;
using RefugeeLand.Core.Api.Brokers.Loggings;
using RefugeeLand.Core.Api.Brokers.Storages;
using RefugeeLand.Core.Api.Models.Refugees;

namespace RefugeeLand.Core.Api.Services.Foundations.Refugees
{
    public partial class RefugeeService : IRefugeeService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public RefugeeService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Refugee> AddRefugeeAsync(Refugee refugee) =>
        TryCatch(async () =>
        {
            ValidateRefugeeOnAdd(refugee);

            return await this.storageBroker.InsertRefugeeAsync(refugee);
        });

        public IQueryable<Refugee> RetreiveAllRefugees() =>
            this.storageBroker.SelectAllRefugees();
    }
}
