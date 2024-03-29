// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using RefugeeLand.Core.Api.Models.ShelterOffers;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.ShelterOffers
{
    public partial class ShelterOfferServiceTests
    {
        [Fact]
        public async Task ShouldModifyShelterOfferAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ShelterOffer randomShelterOffer = CreateRandomModifyShelterOffer(randomDateTimeOffset);
            ShelterOffer inputShelterOffer = randomShelterOffer;
            ShelterOffer storageShelterOffer = inputShelterOffer.DeepClone();
            storageShelterOffer.UpdatedDate = randomShelterOffer.CreatedDate;
            ShelterOffer updatedShelterOffer = inputShelterOffer;
            ShelterOffer expectedShelterOffer = updatedShelterOffer.DeepClone();
            Guid shelterOfferId = inputShelterOffer.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterOfferByIdAsync(shelterOfferId))
                    .ReturnsAsync(storageShelterOffer);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateShelterOfferAsync(inputShelterOffer))
                    .ReturnsAsync(updatedShelterOffer);

            // when
            ShelterOffer actualShelterOffer =
                await this.shelterOfferService.ModifyShelterOfferAsync(inputShelterOffer);

            // then
            actualShelterOffer.Should().BeEquivalentTo(expectedShelterOffer);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterOfferByIdAsync(inputShelterOffer.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShelterOfferAsync(inputShelterOffer),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}