// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Moq;
using RefugeeLand.Core.Api.Brokers.DateTimes;
using RefugeeLand.Core.Api.Brokers.Loggings;
using RefugeeLand.Core.Api.Brokers.Storages;
using RefugeeLand.Core.Api.Models.RefugeeGroups;
using RefugeeLand.Core.Api.Services.Foundations.RefugeeGroups;
using Tynamix.ObjectFiller;

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
        
        private static RefugeeGroup CreateRandomRefugeeGroup() =>
            CreateRefugeeGroupFiller(dates: DateTimeOffset.UtcNow).Create();

        private static RefugeeGroup CreateRandomRefugeeGroup(DateTimeOffset dates) =>
            CreateRefugeeGroupFiller(dates).Create();
        
        private static Filler<RefugeeGroup> CreateRefugeeGroupFiller(DateTimeOffset dates)
        {
            var filler = new Filler<RefugeeGroup>();

            filler.Setup()
                .OnProperty(refugeeGroup => refugeeGroup.ShelterRequests).IgnoreIt()
                .OnProperty(refugeeGroup => refugeeGroup.RefugeeGroupMemberships).IgnoreIt()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }

}