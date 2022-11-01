// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

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
        public async Task ShouldRetrieveShelterOfferByIdAsync()
        {
            // given
            ShelterOffer randomShelterOffer = CreateRandomShelterOffer();
            ShelterOffer inputShelterOffer = randomShelterOffer;
            ShelterOffer storageShelterOffer = randomShelterOffer;
            ShelterOffer expectedShelterOffer = storageShelterOffer.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectShelterOfferByIdAsync(inputShelterOffer.Id))
                    .ReturnsAsync(storageShelterOffer);

            // when
            ShelterOffer actualShelterOffer =
                await this.shelterOfferService.RetrieveShelterOfferByIdAsync(inputShelterOffer.Id);

            // then
            actualShelterOffer.Should().BeEquivalentTo(expectedShelterOffer);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterOfferByIdAsync(inputShelterOffer.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}