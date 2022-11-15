using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using RefugeeLand.Core.Api.Models.ShelterRequests;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.ShelterRequests
{
    public partial class ShelterRequestServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveShelterRequestByIdAsync()
        {
            // given
            ShelterRequest randomShelterRequest = CreateRandomShelterRequest();
            ShelterRequest inputShelterRequest = randomShelterRequest;
            ShelterRequest storageShelterRequest = randomShelterRequest;
            ShelterRequest expectedShelterRequest = storageShelterRequest.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterRequestByIdAsync(inputShelterRequest.Id))
                    .ReturnsAsync(storageShelterRequest);

            // when
            ShelterRequest actualShelterRequest =
                await this.shelterRequestService.RetrieveShelterRequestByIdAsync(inputShelterRequest.Id);

            // then
            actualShelterRequest.Should().BeEquivalentTo(expectedShelterRequest);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(inputShelterRequest.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}