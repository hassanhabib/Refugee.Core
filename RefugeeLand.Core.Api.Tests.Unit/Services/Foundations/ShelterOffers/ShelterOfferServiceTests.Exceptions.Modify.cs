using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using RefugeeLand.Core.Api.Models.ShelterOffers;
using RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.ShelterOffers
{
    public partial class ShelterOfferServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            ShelterOffer randomShelterOffer = CreateRandomShelterOffer();
            SqlException sqlException = GetSqlException();

            var failedShelterOfferStorageException =
                new FailedShelterOfferStorageException(sqlException);

            var expectedShelterOfferDependencyException =
                new ShelterOfferDependencyException(failedShelterOfferStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<ShelterOffer> modifyShelterOfferTask =
                this.shelterOfferService.ModifyShelterOfferAsync(randomShelterOffer);

            ShelterOfferDependencyException actualShelterOfferDependencyException =
                await Assert.ThrowsAsync<ShelterOfferDependencyException>(
                    modifyShelterOfferTask.AsTask);

            // then
            actualShelterOfferDependencyException.Should()
                .BeEquivalentTo(expectedShelterOfferDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterOfferByIdAsync(randomShelterOffer.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedShelterOfferDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShelterOfferAsync(randomShelterOffer),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}