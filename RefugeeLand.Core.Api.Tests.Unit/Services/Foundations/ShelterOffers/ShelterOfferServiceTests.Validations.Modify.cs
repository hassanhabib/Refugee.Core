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
    }
}