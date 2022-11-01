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
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidShelterOfferId = Guid.Empty;

            var invalidShelterOfferException =
                new InvalidShelterOfferException();

            invalidShelterOfferException.AddData(
                key: nameof(ShelterOffer.Id),
                values: "Id is required");

            var expectedShelterOfferValidationException =
                new ShelterOfferValidationException(invalidShelterOfferException);

            // when
            ValueTask<ShelterOffer> removeShelterOfferByIdTask =
                this.shelterOfferService.RemoveShelterOfferByIdAsync(invalidShelterOfferId);

            ShelterOfferValidationException actualShelterOfferValidationException =
                await Assert.ThrowsAsync<ShelterOfferValidationException>(
                    removeShelterOfferByIdTask.AsTask);

            // then
            actualShelterOfferValidationException.Should()
                .BeEquivalentTo(expectedShelterOfferValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteShelterOfferAsync(It.IsAny<ShelterOffer>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}