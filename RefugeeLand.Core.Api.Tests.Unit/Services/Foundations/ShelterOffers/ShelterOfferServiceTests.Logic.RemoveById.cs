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
        public async Task ShouldRemoveShelterOfferByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputShelterOfferId = randomId;
            ShelterOffer randomShelterOffer = CreateRandomShelterOffer();
            ShelterOffer storageShelterOffer = randomShelterOffer;
            ShelterOffer expectedInputShelterOffer = storageShelterOffer;
            ShelterOffer deletedShelterOffer = expectedInputShelterOffer;
            ShelterOffer expectedShelterOffer = deletedShelterOffer.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterOfferByIdAsync(inputShelterOfferId))
                    .ReturnsAsync(storageShelterOffer);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteShelterOfferAsync(expectedInputShelterOffer))
                    .ReturnsAsync(deletedShelterOffer);

            // when
            ShelterOffer actualShelterOffer = await this.shelterOfferService
                .RemoveShelterOfferByIdAsync(inputShelterOfferId);

            // then
            actualShelterOffer.Should().BeEquivalentTo(expectedShelterOffer);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterOfferByIdAsync(inputShelterOfferId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteShelterOfferAsync(expectedInputShelterOffer),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}