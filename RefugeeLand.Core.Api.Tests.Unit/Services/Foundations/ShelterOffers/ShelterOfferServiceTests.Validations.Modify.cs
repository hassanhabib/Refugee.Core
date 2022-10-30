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
                values: "Date is required");

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
            actualShelterOfferValidationException.Should().BeEquivalentTo(expectedShelterOfferValidationException);

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
    }
}