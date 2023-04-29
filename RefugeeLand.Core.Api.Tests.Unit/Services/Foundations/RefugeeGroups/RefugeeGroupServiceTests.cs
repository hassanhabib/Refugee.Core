// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using RefugeeLand.Core.Api.Brokers.DateTimes;
using RefugeeLand.Core.Api.Brokers.Loggings;
using RefugeeLand.Core.Api.Brokers.Storages;
using RefugeeLand.Core.Api.Models.RefugeeGroups;
using RefugeeLand.Core.Api.Services.Foundations.RefugeeGroups;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.RefugeeGroups
{
    public partial class RefugeeGroupServiceTests
    {
    
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IRefugeeGroupService refugeeGroupService;

        public RefugeeGroupServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.refugeeGroupService = new RefugeeGroupService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }
        
        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
        
        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));
        
        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();
        
        private static int GetRandomNegativeNumber() => 
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

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

        private static string GetRandomString() =>
            new MnemonicString().GetValue();
        
        private static string GetRandomMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();
        
        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message
                && (actualException.InnerException as Xeption).DataEquals(expectedException.InnerException.Data);
        }
        
        private static RefugeeGroup CreateRandomRefugeeGroup() =>
            CreateRefugeeGroupFiller(dates: DateTimeOffset.UtcNow).Create();

        private static RefugeeGroup CreateRandomRefugeeGroup(DateTimeOffset dates) =>
            CreateRefugeeGroupFiller(dates).Create();
        
        private static IQueryable<RefugeeGroup> CreateRandomRefugeeGroups(DateTimeOffset dates) =>
            CreateRefugeeGroupFiller(dates).Create(count: GetRandomNumber()).AsQueryable();
        
        private static RefugeeGroup CreateRandomModifyRefugeeGroup(DateTimeOffset dateTimeOffset)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            RefugeeGroup randomRefugeeGroup = CreateRandomRefugeeGroup(dateTimeOffset);

            randomRefugeeGroup.CreatedDate =
                randomRefugeeGroup.CreatedDate.AddDays(randomDaysInPast);

            return randomRefugeeGroup;
        }
        
        private static Filler<RefugeeGroup> CreateRefugeeGroupFiller(DateTimeOffset dates)
        {
            var filler = new Filler<RefugeeGroup>();

            filler.Setup()
                .OnProperty(refugeeGroup => refugeeGroup.ShelterRequests).IgnoreIt()
                .OnProperty(refugeeGroup => refugeeGroup.RefugeeGroupMemberships).IgnoreIt()
                .OnProperty(refugeeGroup => refugeeGroup.RefugeeGroupMainRepresentative).IgnoreIt()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }

}