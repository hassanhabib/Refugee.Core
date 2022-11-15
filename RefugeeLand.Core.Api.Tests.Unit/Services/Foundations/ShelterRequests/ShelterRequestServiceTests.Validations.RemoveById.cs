using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using RefugeeLand.Core.Api.Models.ShelterRequests;
using RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.ShelterRequests
{
    public partial class ShelterRequestServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidShelterRequestId = Guid.Empty;

            var invalidShelterRequestException =
                new InvalidShelterRequestException();

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.Id),
                values: "Id is required");

            var expectedShelterRequestValidationException =
                new ShelterRequestValidationException(invalidShelterRequestException);

            // when
            ValueTask<ShelterRequest> removeShelterRequestByIdTask =
                this.shelterRequestService.RemoveShelterRequestByIdAsync(invalidShelterRequestId);

            ShelterRequestValidationException actualShelterRequestValidationException =
                await Assert.ThrowsAsync<ShelterRequestValidationException>(
                    removeShelterRequestByIdTask.AsTask);

            // then
            actualShelterRequestValidationException.Should()
                .BeEquivalentTo(expectedShelterRequestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteShelterRequestAsync(It.IsAny<ShelterRequest>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}