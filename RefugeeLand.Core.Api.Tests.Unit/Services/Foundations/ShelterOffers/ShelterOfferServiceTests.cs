using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using RefugeeLand.Core.Api.Brokers.DateTimes;
using RefugeeLand.Core.Api.Brokers.Loggings;
using RefugeeLand.Core.Api.Brokers.Storages;
using RefugeeLand.Core.Api.Models.ShelterOffers;
using RefugeeLand.Core.Api.Services.Foundations.ShelterOffers;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.ShelterOffers
{
    public partial class ShelterOfferServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IShelterOfferService shelterOfferService;

        public ShelterOfferServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.shelterOfferService = new ShelterOfferService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

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

        private static ShelterOffer CreateRandomShelterOffer() =>
            CreateShelterOfferFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static ShelterOffer CreateRandomShelterOffer(DateTimeOffset dateTimeOffset) =>
            CreateShelterOfferFiller(dateTimeOffset).Create();

        private static Filler<ShelterOffer> CreateShelterOfferFiller(DateTimeOffset dateTimeOffset)
        {
            Guid userId = Guid.NewGuid();
            var filler = new Filler<ShelterOffer>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(shelterOffer => shelterOffer.CreatedByUserId).Use(userId)
                .OnProperty(shelterOffer => shelterOffer.UpdatedByUserId).Use(userId);

            // TODO: Complete the filler setup e.g. ignore related properties etc...

            return filler;
        }
    }
}