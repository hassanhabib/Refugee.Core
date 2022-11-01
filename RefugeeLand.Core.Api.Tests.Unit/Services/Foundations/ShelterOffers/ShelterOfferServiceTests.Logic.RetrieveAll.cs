// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using RefugeeLand.Core.Api.Models.ShelterOffers;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.ShelterOffers
{
    public partial class ShelterOfferServiceTests
    {
        [Fact]
        public void ShouldReturnShelterOffers()
        {
            // given
            IQueryable<ShelterOffer> randomShelterOffers = CreateRandomShelterOffers();
            IQueryable<ShelterOffer> storageShelterOffers = randomShelterOffers;
            IQueryable<ShelterOffer> expectedShelterOffers = storageShelterOffers;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllShelterOffers())
                    .Returns(storageShelterOffers);

            // when
            IQueryable<ShelterOffer> actualShelterOffers =
                this.shelterOfferService.RetrieveAllShelterOffers();

            // then
            actualShelterOffers.Should().BeEquivalentTo(expectedShelterOffers);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllShelterOffers(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}