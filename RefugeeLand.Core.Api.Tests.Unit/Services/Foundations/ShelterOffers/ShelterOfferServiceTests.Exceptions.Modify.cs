using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            ShelterOffer someShelterOffer = CreateRandomShelterOffer();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidShelterOfferReferenceException =
                new InvalidShelterOfferReferenceException(foreignKeyConstraintConflictException);

            ShelterOfferDependencyValidationException expectedShelterOfferDependencyValidationException =
                new ShelterOfferDependencyValidationException(invalidShelterOfferReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<ShelterOffer> modifyShelterOfferTask =
                this.shelterOfferService.ModifyShelterOfferAsync(someShelterOffer);

            ShelterOfferDependencyValidationException actualShelterOfferDependencyValidationException =
                await Assert.ThrowsAsync<ShelterOfferDependencyValidationException>(
                    modifyShelterOfferTask.AsTask);

            // then
            actualShelterOfferDependencyValidationException.Should()
                .BeEquivalentTo(expectedShelterOfferDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterOfferByIdAsync(someShelterOffer.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedShelterOfferDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShelterOfferAsync(someShelterOffer),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            ShelterOffer randomShelterOffer = CreateRandomShelterOffer();
            var databaseUpdateException = new DbUpdateException();

            var failedShelterOfferStorageException =
                new FailedShelterOfferStorageException(databaseUpdateException);

            var expectedShelterOfferDependencyException =
                new ShelterOfferDependencyException(failedShelterOfferStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

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
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShelterOfferAsync(randomShelterOffer),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            ShelterOffer randomShelterOffer = CreateRandomShelterOffer();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedShelterOfferException =
                new LockedShelterOfferException(databaseUpdateConcurrencyException);

            var expectedShelterOfferDependencyValidationException =
                new ShelterOfferDependencyValidationException(lockedShelterOfferException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<ShelterOffer> modifyShelterOfferTask =
                this.shelterOfferService.ModifyShelterOfferAsync(randomShelterOffer);

            ShelterOfferDependencyValidationException actualShelterOfferDependencyValidationException =
                await Assert.ThrowsAsync<ShelterOfferDependencyValidationException>(
                    modifyShelterOfferTask.AsTask);

            // then
            actualShelterOfferDependencyValidationException.Should()
                .BeEquivalentTo(expectedShelterOfferDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterOfferByIdAsync(randomShelterOffer.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateShelterOfferAsync(randomShelterOffer),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            ShelterOffer randomShelterOffer = CreateRandomShelterOffer();
            var serviceException = new Exception();

            var failedShelterOfferServiceException =
                new FailedShelterOfferServiceException(serviceException);

            var expectedShelterOfferServiceException =
                new ShelterOfferServiceException(failedShelterOfferServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<ShelterOffer> modifyShelterOfferTask =
                this.shelterOfferService.ModifyShelterOfferAsync(randomShelterOffer);

            ShelterOfferServiceException actualShelterOfferServiceException =
                await Assert.ThrowsAsync<ShelterOfferServiceException>(
                    modifyShelterOfferTask.AsTask);

            // then
            actualShelterOfferServiceException.Should()
                .BeEquivalentTo(expectedShelterOfferServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectShelterOfferByIdAsync(randomShelterOffer.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedShelterOfferServiceException))),
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