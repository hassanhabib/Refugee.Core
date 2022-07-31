using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using RefugeeLand.Core.Api.Brokers.DateTimes;
using RefugeeLand.Core.Api.Brokers.Loggings;
using RefugeeLand.Core.Api.Brokers.Storages;
using RefugeeLand.Core.Api.Models.hosts;
using RefugeeLand.Core.Api.Services.Foundations.hosts;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.hosts
{
    public partial class hostServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IhostService hostService;

        public hostServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.hostService = new hostService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        public static TheoryData MinutesBeforeOrAfter()
        {
            int randomNumber = GetRandomNumber();
            int randomNegativeNumber = GetRandomNegativeNumber();

            return new TheoryData<int>
            {
                randomNumber,
                randomNegativeNumber
            };
        }

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static host CreateRandomModifyhost(DateTimeOffset dateTimeOffset)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            host randomhost = CreateRandomhost(dateTimeOffset);

            randomhost.CreatedDate =
                randomhost.CreatedDate.AddDays(randomDaysInPast);

            return randomhost;
        }

        private static IQueryable<host> CreateRandomhosts()
        {
            return CreatehostFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static host CreateRandomhost() =>
            CreatehostFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static host CreateRandomhost(DateTimeOffset dateTimeOffset) =>
            CreatehostFiller(dateTimeOffset).Create();

        private static Filler<host> CreatehostFiller(DateTimeOffset dateTimeOffset)
        {
            Guid userId = Guid.NewGuid();
            var filler = new Filler<host>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(host => host.CreatedByUserId).Use(userId)
                .OnProperty(host => host.UpdatedByUserId).Use(userId);

            // TODO: Complete the filler setup e.g. ignore related properties etc...

            return filler;
        }
    }
}