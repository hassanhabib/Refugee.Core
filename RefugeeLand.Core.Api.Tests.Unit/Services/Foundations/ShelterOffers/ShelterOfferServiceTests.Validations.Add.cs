using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using RefugeeLand.Core.Api.Models.ShelterOffers;
using RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.ShelterOffers
{
    public partial class ShelterOfferServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfShelterOfferIsNullAndLogItAsync()
        {
            // given
            ShelterOffer nullShelterOffer = null;

            var nullShelterOfferException =
                new NullShelterOfferException();

            var expectedShelterOfferValidationException =
                new ShelterOfferValidationException(nullShelterOfferException);

            // when
            ValueTask<ShelterOffer> addShelterOfferTask =
                this.shelterOfferService.AddShelterOfferAsync(nullShelterOffer);

            ShelterOfferValidationException actualShelterOfferValidationException =
                await Assert.ThrowsAsync<ShelterOfferValidationException>(() =>
                    addShelterOfferTask.AsTask());

            // then
            actualShelterOfferValidationException.Should()
                .BeEquivalentTo(expectedShelterOfferValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfShelterOfferIsInvalidAndLogItAsync()
        {
            // given
            ShelterOfferStatus invalidStatus = (ShelterOfferStatus)42; // Forcing out of range enum by casting
            
            var invalidShelterOffer = new ShelterOffer
            {
                Status = invalidStatus
            };

            var invalidShelterOfferException =
                new InvalidShelterOfferException();

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.Id),
                values: "Id is required");

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.StartDate),
                values: "Date is required");
            
            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.EndDate),
                values: "Date is required");
            
            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.Status),
                values: "Value is invalid");
            
            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.ShelterId),
                values: "Id is required");
            
            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.HostId),
                values: "Id is required");

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.CreatedDate),
                values: "Date is required");

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.CreatedByUserId),
                values: "Id is required");

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.UpdatedDate),
                values: "Date is required");

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.UpdatedByUserId),
                values: "Id is required");

            var expectedShelterOfferValidationException =
                new ShelterOfferValidationException(invalidShelterOfferException);

            // when
            ValueTask<ShelterOffer> addShelterOfferTask =
                this.shelterOfferService.AddShelterOfferAsync(invalidShelterOffer);

            ShelterOfferValidationException actualShelterOfferValidationException =
                await Assert.ThrowsAsync<ShelterOfferValidationException>(() =>
                    addShelterOfferTask.AsTask());

            // then
            actualShelterOfferValidationException.Should()
                .BeEquivalentTo(expectedShelterOfferValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShelterOfferAsync(It.IsAny<ShelterOffer>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfStartDateIsLaterThanEndDateAndLogItAsync()
        {
            //given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ShelterOffer randomShelterOffer = CreateRandomShelterOffer(randomDateTimeOffset);
            ShelterOffer invalidShelterOffer = randomShelterOffer;

            invalidShelterOffer.StartDate =
                invalidShelterOffer.StartDate.AddDays(randomNumber);

            var invalidShelterOfferException = new InvalidShelterOfferException();

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.StartDate),
                values: $"Date is later than {nameof(ShelterOffer.EndDate)}");

            var expectedShelterOfferValidationException =
                new ShelterOfferValidationException(invalidShelterOfferException);

            this.dateTimeBrokerMock.Setup(broker => 
               broker.GetCurrentDateTimeOffset())
                   .Returns(randomDateTimeOffset);
            
            //when
            ValueTask<ShelterOffer> addShelterOfferTask =
                this.shelterOfferService.AddShelterOfferAsync(invalidShelterOffer);

            ShelterOfferValidationException actualShelterOfferValidationException =
                await Assert.ThrowsAsync<ShelterOfferValidationException>(() => 
                    addShelterOfferTask.AsTask());

            //then
            actualShelterOfferValidationException.Should()
                .BeEquivalentTo(expectedShelterOfferValidationException);
            
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShelterOfferAsync(It.IsAny<ShelterOffer>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ShelterOffer randomShelterOffer = CreateRandomShelterOffer(randomDateTimeOffset);
            ShelterOffer invalidShelterOffer = randomShelterOffer;

            invalidShelterOffer.UpdatedDate =
                invalidShelterOffer.CreatedDate.AddDays(randomNumber);

            var invalidShelterOfferException = new InvalidShelterOfferException();

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.UpdatedDate),
                values: $"Date is not the same as {nameof(ShelterOffer.CreatedDate)}");

            var expectedShelterOfferValidationException =
                new ShelterOfferValidationException(invalidShelterOfferException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<ShelterOffer> addShelterOfferTask =
                this.shelterOfferService.AddShelterOfferAsync(invalidShelterOffer);

            ShelterOfferValidationException actualShelterOfferValidationException =
                await Assert.ThrowsAsync<ShelterOfferValidationException>(() =>
                    addShelterOfferTask.AsTask());

            // then
            actualShelterOfferValidationException.Should()
                .BeEquivalentTo(expectedShelterOfferValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShelterOfferAsync(It.IsAny<ShelterOffer>()),
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
            ShelterOffer randomShelterOffer = CreateRandomShelterOffer(randomDateTimeOffset);
            ShelterOffer invalidShelterOffer = randomShelterOffer;
            invalidShelterOffer.UpdatedByUserId = Guid.NewGuid();

            var invalidShelterOfferException =
                new InvalidShelterOfferException();

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.UpdatedByUserId),
                values: $"Id is not the same as {nameof(ShelterOffer.CreatedByUserId)}");

            var expectedShelterOfferValidationException =
                new ShelterOfferValidationException(invalidShelterOfferException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<ShelterOffer> addShelterOfferTask =
                this.shelterOfferService.AddShelterOfferAsync(invalidShelterOffer);

            ShelterOfferValidationException actualShelterOfferValidationException =
                await Assert.ThrowsAsync<ShelterOfferValidationException>(() =>
                    addShelterOfferTask.AsTask());

            // then
            actualShelterOfferValidationException.Should()
                .BeEquivalentTo(expectedShelterOfferValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShelterOfferAsync(It.IsAny<ShelterOffer>()),
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

            ShelterOffer randomShelterOffer = CreateRandomShelterOffer(invalidDateTime);
            ShelterOffer invalidShelterOffer = randomShelterOffer;

            var invalidShelterOfferException =
                new InvalidShelterOfferException();

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.CreatedDate),
                values: "Date is not recent");

            var expectedShelterOfferValidationException =
                new ShelterOfferValidationException(invalidShelterOfferException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<ShelterOffer> addShelterOfferTask =
                this.shelterOfferService.AddShelterOfferAsync(invalidShelterOffer);

            ShelterOfferValidationException actualShelterOfferValidationException =
                await Assert.ThrowsAsync<ShelterOfferValidationException>(() =>
                    addShelterOfferTask.AsTask());

            // then
            actualShelterOfferValidationException.Should()
                .BeEquivalentTo(expectedShelterOfferValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShelterOfferAsync(It.IsAny<ShelterOffer>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}