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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidShelterRequestId = Guid.Empty;

            var invalidShelterRequestException =
                new InvalidShelterRequestException();

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.Id),
                values: "Id is required");

            var expectedShelterRequestValidationException =
                new ShelterRequestValidationException(invalidShelterRequestException);

            // when
            ValueTask<ShelterRequest> retrieveShelterRequestByIdTask =
                this.shelterRequestService.RetrieveShelterRequestByIdAsync(invalidShelterRequestId);

            ShelterRequestValidationException actualShelterRequestValidationException =
                await Assert.ThrowsAsync<ShelterRequestValidationException>(
                    retrieveShelterRequestByIdTask.AsTask);

            // then
            actualShelterRequestValidationException.Should()
                .BeEquivalentTo(expectedShelterRequestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfShelterRequestIsNotFoundAndLogItAsync()
        {
            //given
            Guid someShelterRequestId = Guid.NewGuid();
            ShelterRequest noShelterRequest = null;

            var notFoundShelterRequestException =
                new NotFoundShelterRequestException(someShelterRequestId);

            var expectedShelterRequestValidationException =
                new ShelterRequestValidationException(notFoundShelterRequestException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterRequestByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noShelterRequest);

            //when
            ValueTask<ShelterRequest> retrieveShelterRequestByIdTask =
                this.shelterRequestService.RetrieveShelterRequestByIdAsync(someShelterRequestId);

            ShelterRequestValidationException actualShelterRequestValidationException =
                await Assert.ThrowsAsync<ShelterRequestValidationException>(
                    retrieveShelterRequestByIdTask.AsTask);

            //then
            actualShelterRequestValidationException.Should().BeEquivalentTo(expectedShelterRequestValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}