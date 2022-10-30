using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using RefugeeLand.Core.Api.Models.ShelterOffers;
using RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.ShelterOffers
{
    public partial class ShelterOfferServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfShelterOfferIsNullAndLogItAsync()
        {
            // given
            ShelterOffer nullShelterOffer = null;
            var nullShelterOfferException = new NullShelterOfferException();

            var expectedShelterOfferValidationException =
                new ShelterOfferValidationException(nullShelterOfferException);

            // when
            ValueTask<ShelterOffer> modifyShelterOfferTask =
                this.shelterOfferService.ModifyShelterOfferAsync(nullShelterOffer);

            ShelterOfferValidationException actualShelterOfferValidationException =
                await Assert.ThrowsAsync<ShelterOfferValidationException>(
                    modifyShelterOfferTask.AsTask);

            // then
            actualShelterOfferValidationException.Should()
                .BeEquivalentTo(expectedShelterOfferValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShelterOfferAsync(It.IsAny<ShelterOffer>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfShelterOfferIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidShelterOffer = new ShelterOffer
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidShelterOfferException = new InvalidShelterOfferException();

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.Id),
                values: "Id is required");

            //invalidShelterOfferException.AddData(
            //    key: nameof(ShelterOffer.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the ShelterOffer model

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.CreatedDate),
                values: "Date is required");

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.CreatedByUserId),
                values: "Id is required");

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(ShelterOffer.CreatedDate)}"
                });

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.UpdatedByUserId),
                values: "Id is required");

            var expectedShelterOfferValidationException =
                new ShelterOfferValidationException(invalidShelterOfferException);

            // when
            ValueTask<ShelterOffer> modifyShelterOfferTask =
                this.shelterOfferService.ModifyShelterOfferAsync(invalidShelterOffer);

            ShelterOfferValidationException actualShelterOfferValidationException =
                await Assert.ThrowsAsync<ShelterOfferValidationException>(
                    modifyShelterOfferTask.AsTask);

            //then
            actualShelterOfferValidationException.Should()
                .BeEquivalentTo(expectedShelterOfferValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShelterOfferAsync(It.IsAny<ShelterOffer>()),
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
            ShelterOffer randomShelterOffer = CreateRandomShelterOffer(randomDateTimeOffset);
            ShelterOffer invalidShelterOffer = randomShelterOffer;
            var invalidShelterOfferException = new InvalidShelterOfferException();

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.UpdatedDate),
                values: $"Date is the same as {nameof(ShelterOffer.CreatedDate)}");

            var expectedShelterOfferValidationException =
                new ShelterOfferValidationException(invalidShelterOfferException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<ShelterOffer> modifyShelterOfferTask =
                this.shelterOfferService.ModifyShelterOfferAsync(invalidShelterOffer);

            ShelterOfferValidationException actualShelterOfferValidationException =
                await Assert.ThrowsAsync<ShelterOfferValidationException>(
                    modifyShelterOfferTask.AsTask);

            // then
            actualShelterOfferValidationException.Should()
                .BeEquivalentTo(expectedShelterOfferValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterOfferByIdAsync(invalidShelterOffer.Id),
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
            ShelterOffer randomShelterOffer = CreateRandomShelterOffer(randomDateTimeOffset);
            randomShelterOffer.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidShelterOfferException =
                new InvalidShelterOfferException();

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.UpdatedDate),
                values: "Date is not recent");

            var expectedShelterOfferValidatonException =
                new ShelterOfferValidationException(invalidShelterOfferException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<ShelterOffer> modifyShelterOfferTask =
                this.shelterOfferService.ModifyShelterOfferAsync(randomShelterOffer);

            ShelterOfferValidationException actualShelterOfferValidationException =
                await Assert.ThrowsAsync<ShelterOfferValidationException>(
                    modifyShelterOfferTask.AsTask);

            // then
            actualShelterOfferValidationException.Should()
                .BeEquivalentTo(expectedShelterOfferValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterOfferByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfShelterOfferDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ShelterOffer randomShelterOffer = CreateRandomModifyShelterOffer(randomDateTimeOffset);
            ShelterOffer nonExistShelterOffer = randomShelterOffer;
            ShelterOffer nullShelterOffer = null;

            var notFoundShelterOfferException =
                new NotFoundShelterOfferException(nonExistShelterOffer.Id);

            var expectedShelterOfferValidationException =
                new ShelterOfferValidationException(notFoundShelterOfferException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterOfferByIdAsync(nonExistShelterOffer.Id))
                .ReturnsAsync(nullShelterOffer);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<ShelterOffer> modifyShelterOfferTask =
                this.shelterOfferService.ModifyShelterOfferAsync(nonExistShelterOffer);

            ShelterOfferValidationException actualShelterOfferValidationException =
                await Assert.ThrowsAsync<ShelterOfferValidationException>(
                    modifyShelterOfferTask.AsTask);

            // then
            actualShelterOfferValidationException.Should()
                .BeEquivalentTo(expectedShelterOfferValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterOfferByIdAsync(nonExistShelterOffer.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferValidationException))),
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
            ShelterOffer randomShelterOffer = CreateRandomModifyShelterOffer(randomDateTimeOffset);
            ShelterOffer invalidShelterOffer = randomShelterOffer.DeepClone();
            ShelterOffer storageShelterOffer = invalidShelterOffer.DeepClone();
            storageShelterOffer.CreatedDate = storageShelterOffer.CreatedDate.AddMinutes(randomMinutes);
            storageShelterOffer.UpdatedDate = storageShelterOffer.UpdatedDate.AddMinutes(randomMinutes);
            var invalidShelterOfferException = new InvalidShelterOfferException();

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.CreatedDate),
                values: $"Date is not the same as {nameof(ShelterOffer.CreatedDate)}");

            var expectedShelterOfferValidationException =
                new ShelterOfferValidationException(invalidShelterOfferException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterOfferByIdAsync(invalidShelterOffer.Id))
                .ReturnsAsync(storageShelterOffer);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<ShelterOffer> modifyShelterOfferTask =
                this.shelterOfferService.ModifyShelterOfferAsync(invalidShelterOffer);

            ShelterOfferValidationException actualShelterOfferValidationException =
                await Assert.ThrowsAsync<ShelterOfferValidationException>(
                    modifyShelterOfferTask.AsTask);

            // then
            actualShelterOfferValidationException.Should()
                .BeEquivalentTo(expectedShelterOfferValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterOfferByIdAsync(invalidShelterOffer.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedShelterOfferValidationException))),
                       Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}