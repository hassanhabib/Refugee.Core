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
        public async Task ShouldAddShelterOfferAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            ShelterOffer randomShelterOffer = CreateRandomShelterOffer(randomDateTimeOffset);
            ShelterOffer inputShelterOffer = randomShelterOffer;
            ShelterOffer storageShelterOffer = inputShelterOffer;
            ShelterOffer expectedShelterOffer = storageShelterOffer.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertShelterOfferAsync(inputShelterOffer))
                    .ReturnsAsync(storageShelterOffer);

            // when
            ShelterOffer actualShelterOffer = await this.shelterOfferService
                .AddShelterOfferAsync(inputShelterOffer);

            // then
            actualShelterOffer.Should().BeEquivalentTo(expectedShelterOffer);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertShelterOfferAsync(inputShelterOffer),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}