using Moq;
using RefugeeLand.Core.Api.Brokers.DateTimes;
using RefugeeLand.Core.Api.Brokers.Loggings;
using RefugeeLand.Core.Api.Brokers.Storages;
using RefugeeLand.Core.Api.Models.RefugeePets;
using RefugeeLand.Core.Api.Services.Foundations.RefugeePets;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations
{
    public class RefugeePetServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IRefugeePetService refugeePetService;

        public RefugeePetServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.refugeePetService = new RefugeePetService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object
            );
        }

        [Fact]
        public async Task ShouldAddRefugeePet()
        {
            //given arranged values / input
            //random refugeePet
            Guid UserId = Guid.NewGuid();

            var randomRefugeePet = new RefugeePet
            {
                Id = Guid.NewGuid(),
                CreatedByUserId = UserId,
                PetId = Guid.NewGuid(),
                RefugeeId = Guid.NewGuid(),
                CreatedDate = DateTimeOffset.Now,
                UpdatedDate = DateTimeOffset.Now,
                UpdatedByUserId = UserId
            };

            RefugeePet inputRefugeePet = randomRefugeePet;
            RefugeePet insertedRefugeePet = inputRefugeePet;
            RefugeePet expectedRefugeePet = insertedRefugeePet;
           
            //Todo: setup?
            this.dateTimeBrokerMock.Setup( broker => 
                broker.GetCurrentDateTimeOffset())
                

                //when action is performed
                // = refugeePetService.AddRefugeePetAsync(inputRefugeePet);


            //then we should assert the expected outcome is true

            //finally clean for other calls
        }
    }
}