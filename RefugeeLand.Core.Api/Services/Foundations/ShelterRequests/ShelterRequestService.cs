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
        public async Task ShouldModifyShelterRequestAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ShelterRequest randomShelterRequest = CreateRandomModifyShelterRequest(randomDateTimeOffset);
            ShelterRequest inputShelterRequest = randomShelterRequest;
            ShelterRequest storageShelterRequest = inputShelterRequest.DeepClone();
            storageShelterRequest.UpdatedDate = randomShelterRequest.CreatedDate;
            ShelterRequest updatedShelterRequest = inputShelterRequest;
            ShelterRequest expectedShelterRequest = updatedShelterRequest.DeepClone();
            Guid shelterRequestId = inputShelterRequest.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterRequestByIdAsync(shelterRequestId))
                    .ReturnsAsync(storageShelterRequest);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateShelterRequestAsync(inputShelterRequest))
                    .ReturnsAsync(updatedShelterRequest);

            // when
            ShelterRequest actualShelterRequest =
                await this.shelterRequestService.ModifyShelterRequestAsync(inputShelterRequest);

            // then
            actualShelterRequest.Should().BeEquivalentTo(expectedShelterRequest);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(inputShelterRequest.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShelterRequestAsync(inputShelterRequest),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}