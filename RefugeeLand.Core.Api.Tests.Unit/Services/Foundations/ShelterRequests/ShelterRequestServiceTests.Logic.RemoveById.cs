using System;
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
        public async Task ShouldRemoveShelterRequestByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputShelterRequestId = randomId;
            ShelterRequest randomShelterRequest = CreateRandomShelterRequest();
            ShelterRequest storageShelterRequest = randomShelterRequest;
            ShelterRequest expectedInputShelterRequest = storageShelterRequest;
            ShelterRequest deletedShelterRequest = expectedInputShelterRequest;
            ShelterRequest expectedShelterRequest = deletedShelterRequest.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterRequestByIdAsync(inputShelterRequestId))
                    .ReturnsAsync(storageShelterRequest);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteShelterRequestAsync(expectedInputShelterRequest))
                    .ReturnsAsync(deletedShelterRequest);

            // when
            ShelterRequest actualShelterRequest = await this.shelterRequestService
                .RemoveShelterRequestByIdAsync(inputShelterRequestId);

            // then
            actualShelterRequest.Should().BeEquivalentTo(expectedShelterRequest);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(inputShelterRequestId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteShelterRequestAsync(expectedInputShelterRequest),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}