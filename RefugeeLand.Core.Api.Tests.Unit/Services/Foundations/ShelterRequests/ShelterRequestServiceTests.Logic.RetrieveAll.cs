using System.Linq;
using FluentAssertions;
using Moq;
using RefugeeLand.Core.Api.Models.ShelterRequests;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.ShelterRequests
{
    public partial class ShelterRequestServiceTests
    {
        [Fact]
        public void ShouldReturnShelterRequests()
        {
            // given
            IQueryable<ShelterRequest> randomShelterRequests = CreateRandomShelterRequests();
            IQueryable<ShelterRequest> storageShelterRequests = randomShelterRequests;
            IQueryable<ShelterRequest> expectedShelterRequests = storageShelterRequests;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllShelterRequests())
                    .Returns(storageShelterRequests);

            // when
            IQueryable<ShelterRequest> actualShelterRequests =
                this.shelterRequestService.RetrieveAllShelterRequests();

            // then
            actualShelterRequests.Should().BeEquivalentTo(expectedShelterRequests);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllShelterRequests(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}