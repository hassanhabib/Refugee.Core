// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidShelterOfferId = Guid.Empty;

            var invalidShelterOfferException =
                new InvalidShelterOfferException();

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.Id),
                values: "Id is required");

            var expectedShelterOfferValidationException =
                new ShelterOfferValidationException(invalidShelterOfferException);

            // when
            ValueTask<ShelterOffer> retrieveShelterOfferByIdTask =
                this.shelterOfferService.RetrieveShelterOfferByIdAsync(invalidShelterOfferId);

            ShelterOfferValidationException actualShelterOfferValidationException =
                await Assert.ThrowsAsync<ShelterOfferValidationException>(
                    retrieveShelterOfferByIdTask.AsTask);

            // then
            actualShelterOfferValidationException.Should()
                .BeEquivalentTo(expectedShelterOfferValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterOfferByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfShelterOfferIsNotFoundAndLogItAsync()
        {
            //given
            Guid someShelterOfferId = Guid.NewGuid();
            ShelterOffer noShelterOffer = null;

            var notFoundShelterOfferException =
                new NotFoundShelterOfferException(someShelterOfferId);

            var expectedShelterOfferValidationException =
                new ShelterOfferValidationException(notFoundShelterOfferException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterOfferByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noShelterOffer);

            //when
            ValueTask<ShelterOffer> retrieveShelterOfferByIdTask =
                this.shelterOfferService.RetrieveShelterOfferByIdAsync(someShelterOfferId);

            ShelterOfferValidationException actualShelterOfferValidationException =
                await Assert.ThrowsAsync<ShelterOfferValidationException>(
                    retrieveShelterOfferByIdTask.AsTask);

            //then
            actualShelterOfferValidationException.Should().BeEquivalentTo(expectedShelterOfferValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterOfferByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}