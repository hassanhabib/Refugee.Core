using Microsoft.Data.SqlClient;
using Moq;
using RefugeeLand.Core.Api.Brokers.DateTimes;
using RefugeeLand.Core.Api.Brokers.Loggings;
using RefugeeLand.Core.Api.Brokers.Storages;
using RefugeeLand.Core.Api.Models.ShelterRequests;
using RefugeeLand.Core.Api.Services.Foundations.ShelterRequests;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.ShelterRequests
{
    public partial class ShelterRequestServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IShelterRequestService shelterRequestService;

        public ShelterRequestServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.shelterRequestService = new ShelterRequestService(
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

        private static ShelterRequest CreateRandomModifyShelterRequest(DateTimeOffset dateTimeOffset)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            ShelterRequest randomShelterRequest = CreateRandomShelterRequest(dateTimeOffset);

            randomShelterRequest.CreatedDate =
                randomShelterRequest.CreatedDate.AddDays(randomDaysInPast);

            return randomShelterRequest;
        }

        private static IQueryable<ShelterRequest> CreateRandomShelterRequests()
        {
            return CreateShelterRequestFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static ShelterRequest CreateRandomShelterRequest() =>
            CreateShelterRequestFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static ShelterRequest CreateRandomShelterRequest(DateTimeOffset dateTimeOffset) =>
            CreateShelterRequestFiller(dateTimeOffset).Create();

        private static Filler<ShelterRequest> CreateShelterRequestFiller(DateTimeOffset dateTimeOffset)
        {
            Guid userId = Guid.NewGuid();
            var filler = new Filler<ShelterRequest>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(shelterRequest => shelterRequest.CreatedByUserId).Use(userId)
                .OnProperty(shelterRequest => shelterRequest.UpdatedByUserId).Use(userId)
                .OnProperty(shelterRequest => shelterRequest.ShelterOffer).IgnoreIt()
                .OnProperty(shelterRequest => shelterRequest.RefugeeGroup).IgnoreIt()
                .OnProperty(shelterRequest => shelterRequest.RefugeeApplicant).IgnoreIt();

            return filler;
        }
    }
}