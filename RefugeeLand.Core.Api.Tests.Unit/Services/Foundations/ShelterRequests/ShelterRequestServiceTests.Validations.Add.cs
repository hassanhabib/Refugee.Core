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
        public async Task ShouldThrowValidationExceptionOnAddIfShelterRequestIsNullAndLogItAsync()
        {
            // given
            ShelterRequest nullShelterRequest = null;

            var nullShelterRequestException =
                new NullShelterRequestException();

            var expectedShelterRequestValidationException =
                new ShelterRequestValidationException(nullShelterRequestException);

            // when
            ValueTask<ShelterRequest> addShelterRequestTask =
                this.shelterRequestService.AddShelterRequestAsync(nullShelterRequest);

            ShelterRequestValidationException actualShelterRequestValidationException =
                await Assert.ThrowsAsync<ShelterRequestValidationException>(() =>
                    addShelterRequestTask.AsTask());

            // then
            actualShelterRequestValidationException.Should()
                .BeEquivalentTo(expectedShelterRequestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfShelterRequestIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidShelterRequest = new ShelterRequest
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidShelterRequestException =
                new InvalidShelterRequestException();

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.Id),
                values: "Id is required");

            //invalidShelterRequestException.AddData(
            //    key: nameof(ShelterRequest.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the ShelterRequest model

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.CreatedDate),
                values: "Date is required");

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.CreatedByUserId),
                values: "Id is required");

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.UpdatedDate),
                values: "Date is required");

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.UpdatedByUserId),
                values: "Id is required");

            var expectedShelterRequestValidationException =
                new ShelterRequestValidationException(invalidShelterRequestException);

            // when
            ValueTask<ShelterRequest> addShelterRequestTask =
                this.shelterRequestService.AddShelterRequestAsync(invalidShelterRequest);

            ShelterRequestValidationException actualShelterRequestValidationException =
                await Assert.ThrowsAsync<ShelterRequestValidationException>(() =>
                    addShelterRequestTask.AsTask());

            // then
            actualShelterRequestValidationException.Should()
                .BeEquivalentTo(expectedShelterRequestValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShelterRequestAsync(It.IsAny<ShelterRequest>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ShelterRequest randomShelterRequest = CreateRandomShelterRequest(randomDateTimeOffset);
            ShelterRequest invalidShelterRequest = randomShelterRequest;

            invalidShelterRequest.UpdatedDate =
                invalidShelterRequest.CreatedDate.AddDays(randomNumber);

            var invalidShelterRequestException = new InvalidShelterRequestException();

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.UpdatedDate),
                values: $"Date is not the same as {nameof(ShelterRequest.CreatedDate)}");

            var expectedShelterRequestValidationException =
                new ShelterRequestValidationException(invalidShelterRequestException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<ShelterRequest> addShelterRequestTask =
                this.shelterRequestService.AddShelterRequestAsync(invalidShelterRequest);

            ShelterRequestValidationException actualShelterRequestValidationException =
                await Assert.ThrowsAsync<ShelterRequestValidationException>(() =>
                    addShelterRequestTask.AsTask());

            // then
            actualShelterRequestValidationException.Should()
                .BeEquivalentTo(expectedShelterRequestValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShelterRequestAsync(It.IsAny<ShelterRequest>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUserIdsIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ShelterRequest randomShelterRequest = CreateRandomShelterRequest(randomDateTimeOffset);
            ShelterRequest invalidShelterRequest = randomShelterRequest;
            invalidShelterRequest.UpdatedByUserId = Guid.NewGuid();

            var invalidShelterRequestException =
                new InvalidShelterRequestException();

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.UpdatedByUserId),
                values: $"Id is not the same as {nameof(ShelterRequest.CreatedByUserId)}");

            var expectedShelterRequestValidationException =
                new ShelterRequestValidationException(invalidShelterRequestException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<ShelterRequest> addShelterRequestTask =
                this.shelterRequestService.AddShelterRequestAsync(invalidShelterRequest);

            ShelterRequestValidationException actualShelterRequestValidationException =
                await Assert.ThrowsAsync<ShelterRequestValidationException>(() =>
                    addShelterRequestTask.AsTask());

            // then
            actualShelterRequestValidationException.Should()
                .BeEquivalentTo(expectedShelterRequestValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShelterRequestAsync(It.IsAny<ShelterRequest>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            DateTimeOffset invalidDateTime =
                randomDateTimeOffset.AddMinutes(minutesBeforeOrAfter);

            ShelterRequest randomShelterRequest = CreateRandomShelterRequest(invalidDateTime);
            ShelterRequest invalidShelterRequest = randomShelterRequest;

            var invalidShelterRequestException =
                new InvalidShelterRequestException();

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.CreatedDate),
                values: "Date is not recent");

            var expectedShelterRequestValidationException =
                new ShelterRequestValidationException(invalidShelterRequestException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<ShelterRequest> addShelterRequestTask =
                this.shelterRequestService.AddShelterRequestAsync(invalidShelterRequest);

            ShelterRequestValidationException actualShelterRequestValidationException =
                await Assert.ThrowsAsync<ShelterRequestValidationException>(() =>
                    addShelterRequestTask.AsTask());

            // then
            actualShelterRequestValidationException.Should()
                .BeEquivalentTo(expectedShelterRequestValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShelterRequestAsync(It.IsAny<ShelterRequest>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}