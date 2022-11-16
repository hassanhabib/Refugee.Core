// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using RefugeeLand.Core.Api.Models.ShelterRequests;
using RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.ShelterRequests
{
    public partial class ShelterRequestServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfShelterRequestIsNullAndLogItAsync()
        {
            // given
            ShelterRequest nullShelterRequest = null;
            var nullShelterRequestException = new NullShelterRequestException();

            var expectedShelterRequestValidationException =
                new ShelterRequestValidationException(nullShelterRequestException);

            // when
            ValueTask<ShelterRequest> modifyShelterRequestTask =
                this.shelterRequestService.ModifyShelterRequestAsync(nullShelterRequest);

            ShelterRequestValidationException actualShelterRequestValidationException =
                await Assert.ThrowsAsync<ShelterRequestValidationException>(
                    modifyShelterRequestTask.AsTask);

            // then
            actualShelterRequestValidationException.Should()
                .BeEquivalentTo(expectedShelterRequestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShelterRequestAsync(It.IsAny<ShelterRequest>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfShelterRequestIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            Guid invalidId = Guid.Empty;
            ShelterRequestStatus invalidStatus = (ShelterRequestStatus)42;
            DateTimeOffset invalidDateTime = default; 
            
            var invalidShelterRequest = new ShelterRequest
            {
                Id = invalidId,
                StartDate = invalidDateTime,
                EndDate = invalidDateTime,
                Status = invalidStatus,
                ShelterOfferId = invalidId,
                RefugeeGroupId = invalidId,
                RefugeeApplicantId = invalidId,
                CreatedDate = invalidDateTime,  
                UpdatedDate = invalidDateTime,
                CreatedByUserId = invalidId,
                UpdatedByUserId = invalidId
            };

            var invalidShelterRequestException = new InvalidShelterRequestException();

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.Id),
                values: "Id is required");

             invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.StartDate),
                values: "Date is required");

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.EndDate),
                values: "Date is required");

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.Status),
                values: "Value is invalid");

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.ShelterOfferId),
                values: "Id is required");

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.RefugeeGroupId),
                values: "Id is required");

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.RefugeeApplicantId),
                values: "Id is required");

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.CreatedDate),
                values: "Date is required");

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.CreatedByUserId),
                values: "Id is required");

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(ShelterRequest.CreatedDate)}"
                });

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.UpdatedByUserId),
                values: "Id is required");

            var expectedShelterRequestValidationException =
                new ShelterRequestValidationException(invalidShelterRequestException);

            // when
            ValueTask<ShelterRequest> modifyShelterRequestTask =
                this.shelterRequestService.ModifyShelterRequestAsync(invalidShelterRequest);

            ShelterRequestValidationException actualShelterRequestValidationException =
                await Assert.ThrowsAsync<ShelterRequestValidationException>(
                    modifyShelterRequestTask.AsTask);

            //then
            actualShelterRequestValidationException.Should()
                .BeEquivalentTo(expectedShelterRequestValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShelterRequestAsync(It.IsAny<ShelterRequest>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ShelterRequest randomShelterRequest = CreateRandomShelterRequest(randomDateTimeOffset);
            ShelterRequest invalidShelterRequest = randomShelterRequest;
            var invalidShelterRequestException = new InvalidShelterRequestException();

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.UpdatedDate),
                values: $"Date is the same as {nameof(ShelterRequest.CreatedDate)}");

            var expectedShelterRequestValidationException =
                new ShelterRequestValidationException(invalidShelterRequestException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<ShelterRequest> modifyShelterRequestTask =
                this.shelterRequestService.ModifyShelterRequestAsync(invalidShelterRequest);

            ShelterRequestValidationException actualShelterRequestValidationException =
                await Assert.ThrowsAsync<ShelterRequestValidationException>(
                    modifyShelterRequestTask.AsTask);

            // then
            actualShelterRequestValidationException.Should()
                .BeEquivalentTo(expectedShelterRequestValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(invalidShelterRequest.Id),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ShelterRequest randomShelterRequest = CreateRandomShelterRequest(randomDateTimeOffset);
            randomShelterRequest.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidShelterRequestException =
                new InvalidShelterRequestException();

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.UpdatedDate),
                values: "Date is not recent");

            var expectedShelterRequestValidatonException =
                new ShelterRequestValidationException(invalidShelterRequestException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<ShelterRequest> modifyShelterRequestTask =
                this.shelterRequestService.ModifyShelterRequestAsync(randomShelterRequest);

            ShelterRequestValidationException actualShelterRequestValidationException =
                await Assert.ThrowsAsync<ShelterRequestValidationException>(
                    modifyShelterRequestTask.AsTask);

            // then
            actualShelterRequestValidationException.Should()
                .BeEquivalentTo(expectedShelterRequestValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfShelterRequestDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ShelterRequest randomShelterRequest = CreateRandomModifyShelterRequest(randomDateTimeOffset);
            ShelterRequest nonExistShelterRequest = randomShelterRequest;
            ShelterRequest nullShelterRequest = null;

            var notFoundShelterRequestException =
                new NotFoundShelterRequestException(nonExistShelterRequest.Id);

            var expectedShelterRequestValidationException =
                new ShelterRequestValidationException(notFoundShelterRequestException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterRequestByIdAsync(nonExistShelterRequest.Id))
                .ReturnsAsync(nullShelterRequest);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<ShelterRequest> modifyShelterRequestTask =
                this.shelterRequestService.ModifyShelterRequestAsync(nonExistShelterRequest);

            ShelterRequestValidationException actualShelterRequestValidationException =
                await Assert.ThrowsAsync<ShelterRequestValidationException>(
                    modifyShelterRequestTask.AsTask);

            // then
            actualShelterRequestValidationException.Should()
                .BeEquivalentTo(expectedShelterRequestValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(nonExistShelterRequest.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ShelterRequest randomShelterRequest = CreateRandomModifyShelterRequest(randomDateTimeOffset);
            ShelterRequest invalidShelterRequest = randomShelterRequest.DeepClone();
            ShelterRequest storageShelterRequest = invalidShelterRequest.DeepClone();
            storageShelterRequest.CreatedDate = storageShelterRequest.CreatedDate.AddMinutes(randomMinutes);
            storageShelterRequest.UpdatedDate = storageShelterRequest.UpdatedDate.AddMinutes(randomMinutes);
            var invalidShelterRequestException = new InvalidShelterRequestException();

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.CreatedDate),
                values: $"Date is not the same as {nameof(ShelterRequest.CreatedDate)}");

            var expectedShelterRequestValidationException =
                new ShelterRequestValidationException(invalidShelterRequestException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterRequestByIdAsync(invalidShelterRequest.Id))
                .ReturnsAsync(storageShelterRequest);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<ShelterRequest> modifyShelterRequestTask =
                this.shelterRequestService.ModifyShelterRequestAsync(invalidShelterRequest);

            ShelterRequestValidationException actualShelterRequestValidationException =
                await Assert.ThrowsAsync<ShelterRequestValidationException>(
                    modifyShelterRequestTask.AsTask);

            // then
            actualShelterRequestValidationException.Should()
                .BeEquivalentTo(expectedShelterRequestValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(invalidShelterRequest.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedShelterRequestValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserIdDontMacthStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ShelterRequest randomShelterRequest = CreateRandomModifyShelterRequest(randomDateTimeOffset);
            ShelterRequest invalidShelterRequest = randomShelterRequest.DeepClone();
            ShelterRequest storageShelterRequest = invalidShelterRequest.DeepClone();
            invalidShelterRequest.CreatedByUserId = Guid.NewGuid();
            storageShelterRequest.UpdatedDate = storageShelterRequest.CreatedDate;

            var invalidShelterRequestException = new InvalidShelterRequestException();

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.CreatedByUserId),
                values: $"Id is not the same as {nameof(ShelterRequest.CreatedByUserId)}");

            var expectedShelterRequestValidationException =
                new ShelterRequestValidationException(invalidShelterRequestException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterRequestByIdAsync(invalidShelterRequest.Id))
                .ReturnsAsync(storageShelterRequest);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<ShelterRequest> modifyShelterRequestTask =
                this.shelterRequestService.ModifyShelterRequestAsync(invalidShelterRequest);

            ShelterRequestValidationException actualShelterRequestValidationException =
                await Assert.ThrowsAsync<ShelterRequestValidationException>(
                    modifyShelterRequestTask.AsTask);

            // then
            actualShelterRequestValidationException.Should().BeEquivalentTo(expectedShelterRequestValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(invalidShelterRequest.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedShelterRequestValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ShelterRequest randomShelterRequest = CreateRandomModifyShelterRequest(randomDateTimeOffset);
            ShelterRequest invalidShelterRequest = randomShelterRequest;
            ShelterRequest storageShelterRequest = randomShelterRequest.DeepClone();

            var invalidShelterRequestException = new InvalidShelterRequestException();

            invalidShelterRequestException.AddData(
                key: nameof(ShelterRequest.UpdatedDate),
                values: $"Date is the same as {nameof(ShelterRequest.UpdatedDate)}");

            var expectedShelterRequestValidationException =
                new ShelterRequestValidationException(invalidShelterRequestException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterRequestByIdAsync(invalidShelterRequest.Id))
                .ReturnsAsync(storageShelterRequest);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<ShelterRequest> modifyShelterRequestTask =
                this.shelterRequestService.ModifyShelterRequestAsync(invalidShelterRequest);

            // then
            await Assert.ThrowsAsync<ShelterRequestValidationException>(
                modifyShelterRequestTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterRequestValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterRequestByIdAsync(invalidShelterRequest.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}